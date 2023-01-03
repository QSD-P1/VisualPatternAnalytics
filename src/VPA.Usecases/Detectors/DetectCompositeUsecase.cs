﻿using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using VPA.Domain.Enums;
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
			var classesPerParent = ClassHelperUsecase.GetClassesPerParentClass(projectNode);
			var classesWithParentListType = ClassHelperUsecase.GetClassesWithParentListType(classesPerParent);
			if (!classesWithParentListType.Any())
				return resultCollection;

			// Now we have found the composite class, try and find the leaf within the same interface
			// and check if they have no collection making them a composite instead of a leaf
			foreach (KeyValuePair<string, List<ClassNode>> entry in classesWithParentListType)
			{
				var currentParent = entry.Key;
				var currentClasses = entry.Value;

				var otherClassesForInterface = classesPerParent[currentParent].Where(
					x => !currentClasses.Contains(x)).ToList();

				if (!otherClassesForInterface.Any()) continue;

				foreach (ClassNode classNode in otherClassesForInterface)
				{
					if (classNode.Children != null && !classNode.Children.Any()) continue;

					var fields = classNode.Children.OfType<FieldNode>().ToList();
					bool leafHasCollectionOfParent = false;

					if (fields.Any())
					{
						foreach (var field in fields)
						{
							var collectionGenericObject = FieldHelper.GetCollectionGenericObject(field.Type);

							if (collectionGenericObject != null && Enum.IsDefined(typeof(CollectionTypesEnum), collectionGenericObject.CollectionType) &&
							    collectionGenericObject.GenericType == currentParent)
								leafHasCollectionOfParent = true;
						}
					}

					if (leafHasCollectionOfParent)
					{
						continue;
					}

					// This set classes is a composite because we found a leaf!

					var result = new DetectorResult();
					result.Items.Add(new DetectedItem { MainNode = classNode });

					foreach (ClassNode composite in classesWithParentListType[currentParent])
					{
						result.Items.Add(new DetectedItem { MainNode = composite });
					}

					resultCollection.Results.Add(result);
				}
			}

			return resultCollection;
		}
	}
}