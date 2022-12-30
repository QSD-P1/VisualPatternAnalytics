﻿using System;
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

			// Finding all interfaces that are used
			var allDistinctInterfaces = ClassHelperUsecase.GetUsedClassInterfaces(projectNode);

			// No interfaces in use
			if (allDistinctInterfaces.Count == 0)
				return resultCollection;

			// Combine all classes per interface
			var classesPerInterface = new Dictionary<string, List<ClassNode>>();

			foreach (string distinctInterface in allDistinctInterfaces)
			{
				foreach (ClassNode classNode in projectNode.ClassNodes.Where(
					         x => x.Interfaces.Contains(distinctInterface)))
				{
					if (!classesPerInterface.ContainsKey(distinctInterface))
						classesPerInterface[distinctInterface] = new List<ClassNode>();

					classesPerInterface[distinctInterface].Add(classNode);
				}
			}

			// Check if class has collection field of interface type
			var classesWithInterfaceListType = new Dictionary<string, List<ClassNode>>();

			foreach (KeyValuePair<string, List<ClassNode>> entry in classesPerInterface)
			{
				var currentInterface = entry.Key;

				foreach (ClassNode classNode in entry.Value)
				{
					if (classNode.Children == null) continue;

					var fields = classNode.Children.OfType<FieldNode>().ToList();

					if (!fields.Any()) continue;

					foreach (FieldNode field in fields)
					{
						if (field.Type != $"List<{currentInterface}>") continue;

						// Found it!
						if (!classesWithInterfaceListType.ContainsKey(currentInterface))
							classesWithInterfaceListType[currentInterface] = new List<ClassNode>();

						classesWithInterfaceListType[currentInterface].Add(classNode);
					}
				}
			}

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