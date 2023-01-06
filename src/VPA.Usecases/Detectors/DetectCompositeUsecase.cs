using System;
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
		private readonly IGetClassesPerParentClassUsecase _getClassesPerParentClass;
		private readonly IGetClassesPerInterfaceUsecase _getClassesPerInterface;
		private readonly IGetClassesWithParentListTypeUsecase _getClassesWithParentListType;
		private readonly IGetCollectionGenericObjectUsecase _getCollectionGenericObject;
		public string PatternName => "Composite";

		public DetectCompositeUsecase(
			IGetClassesPerParentClassUsecase getClassesPerParentClass,
			IGetClassesPerInterfaceUsecase getClassesPerInterface,
			IGetClassesWithParentListTypeUsecase getClassesWithParentListType,
			IGetCollectionGenericObjectUsecase collectionGenericObject
			)
		{
			_getClassesPerParentClass = getClassesPerParentClass;
			_getClassesPerInterface = getClassesPerInterface;
			_getClassesWithParentListType = getClassesWithParentListType;
			_getCollectionGenericObject = collectionGenericObject;
		}

		public async Task<DetectionResultCollection> Detect(ProjectNode projectNode)
		{
			var resultCollection = new DetectionResultCollection(PatternName);

			// Combine all classes per interface and abstract classes
			var classesPerParent = _getClassesPerParentClass.Execute(projectNode);
			var classesPerInterface = _getClassesPerInterface.Execute(projectNode);

			var combinedParentAndInterfaceClasses = classesPerParent.Concat(classesPerInterface).ToDictionary(x => x.Key, x => x.Value);

			var classesWithParentListType = _getClassesWithParentListType.Execute(combinedParentAndInterfaceClasses);
			
			if (!classesWithParentListType.Any())
				return resultCollection;

			// Now we have found the composite class, try and find the leaf within the same interface
			// and check if they have no collection making them a composite instead of a leaf
			foreach (KeyValuePair<string, List<ClassNode>> entry in classesWithParentListType)
			{
				var currentParent = entry.Key;
				var currentClasses = entry.Value;

				var otherClassesForInterface = combinedParentAndInterfaceClasses[currentParent].Where(
					x => !currentClasses.Contains(x)).ToList();

				if (!otherClassesForInterface.Any()) continue;

				foreach (ClassNode classNode in otherClassesForInterface)
				{
					if (classNode.Children != null)
					{
						var fields = classNode.Children.OfType<FieldNode>().ToList();
						bool leafHasCollectionOfParent = false;

						if (fields.Any())
						{
							foreach (var field in fields)
							{
								var collectionGenericObject = _getCollectionGenericObject.Execute(field.Type);

								if (collectionGenericObject != null && Enum.IsDefined(typeof(CollectionTypesEnum), collectionGenericObject.CollectionType) &&
								    collectionGenericObject.GenericType == currentParent)
									leafHasCollectionOfParent = true;
							}
						}

						if (leafHasCollectionOfParent)
						{
							continue;
						}
					}

					// This set classes is a composite because we found a leaf!

					var result = new DetectionResult($"{PatternName}{resultCollection.Results.Count + 1}");
					result.DetectedItems.Add(new DetectedItem { MainNode = classNode});
					foreach (ClassNode composite in classesWithParentListType[currentParent])
					{
						result.DetectedItems.Add(new DetectedItem { MainNode = composite });
					}

					resultCollection.Results.Add(result);
				}
			}

			return resultCollection;
		}
	}
}