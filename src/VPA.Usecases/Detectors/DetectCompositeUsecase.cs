using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using VPA.Domain.Models;
using VPA.Usecases.DetectionHelpers;
using VPA.Usecases.Interfaces;

namespace VPA.Usecases.Detectors
{
	public class DetectCompositeUsecase : IDetectCompositeUsecase
	{
		public string PatternName => "Composite";

		public DetectCompositeUsecase()
		{
		}

		public async Task<DetectorResultCollection> Detect(ProjectNode projectNode)
		{
			var resultCollection = new DetectorResultCollection()
			{
				Name = PatternName,
				Results = new List<DetectorResult>(),
			};

			// No classes
			if (projectNode.ClassNodes == null)
				return resultCollection;

			// Combine all classes per interface
			var classesPerInterface = ClassHelperUsecase.GetClassesPerParentClass(projectNode);
			var classesWithInterfaceListType = ClassHelperUsecase.GetClassesWithInterfaceListType(classesPerInterface);


			if (!classesWithInterfaceListType.Any())
				return resultCollection;

			// Now we have found the composite class, try and find the leaf within the same interface
			// and check if they have no collection making them a composite instead of a leaf
			foreach (KeyValuePair<string, List<ClassNode>> entry in classesWithInterfaceListType)
			{
				var currentInterface = entry.Key;
				var currentClasses = entry.Value;

				var otherClassesForInterface = classesPerInterface[currentInterface].Where(
					x => !currentClasses.Contains(x)).ToList();

				if (!otherClassesForInterface.Any()) continue;

				foreach (ClassNode classNode in otherClassesForInterface)
				{
					if (classNode.Children != null && !classNode.Children.Any()) continue;

					var fields = classNode.Children.OfType<FieldNode>().ToList();

					if (fields.Any()) continue;

					if (fields.Any(x => x.Type == $"List<{currentInterface}>")) continue;

					// This set classes is a composite because we found a leaf!

					var result = new DetectorResult();
					result.Items.Add(new DetectedItem() { MainNode = classNode });

					foreach (ClassNode composite in classesWithInterfaceListType[currentInterface])
					{
						result.Items.Add(new DetectedItem() { MainNode = composite });
					}

					resultCollection.Results.Add(result);
				}
			}

			return resultCollection;
		}
	}
}