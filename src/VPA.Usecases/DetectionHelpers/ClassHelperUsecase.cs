using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using VPA.Domain.Enums;
using VPA.Domain.Models;

namespace VPA.Usecases.DetectionHelpers
{
	public static class ClassHelperUsecase
	{
		public static Dictionary<string, List<ClassNode>> GetClassesPerParentClass(ProjectNode projectNode)
		{
			var classesPerParentClass = new Dictionary<string, List<ClassNode>>();

			if (projectNode.ClassNodes == null)
				return classesPerParentClass;

			foreach (var classNode in projectNode.ClassNodes)
			{
				if (classNode.ParentClassName == null)
					continue;

				if (classesPerParentClass.ContainsKey(classNode.ParentClassName))
				{
					classesPerParentClass[classNode.ParentClassName].Add(classNode);
				}
				else
				{
					classesPerParentClass.Add(classNode.ParentClassName, new List<ClassNode> { classNode });
				}
			}

			return classesPerParentClass;
		}

		public static Dictionary<string, List<ClassNode>> GetClassesWithParentListType(Dictionary<string, List<ClassNode>> classesPerParent)
		{
			// Check if class has collection field of interface type
			var classesWithParentListType = new Dictionary<string, List<ClassNode>>();

			foreach (KeyValuePair<string, List<ClassNode>> entry in classesPerParent)
			{
				var currentParent = entry.Key;

				foreach (ClassNode classNode in entry.Value)
				{
					if (classNode.Children == null) continue;

					var fields = classNode.Children.OfType<FieldNode>().ToList();

					if (!fields.Any()) continue;

					foreach (FieldNode field in fields)
					{
						var collectionGenericObject = FieldHelper.GetCollectionGenericObject(field.Type);

						if (!Enum.IsDefined(typeof(CollectionTypesEnum), collectionGenericObject.CollectionType) || collectionGenericObject.GenericType != currentParent) continue;

						// Found it!
						if (!classesWithParentListType.ContainsKey(currentParent))
							classesWithParentListType[currentParent] = new List<ClassNode>();

						classesWithParentListType[currentParent].Add(classNode);
					}
				}
			}

			return classesWithParentListType;
		}
	}
}
