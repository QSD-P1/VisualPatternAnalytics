using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VPA.Domain.Enums;
using VPA.Domain.Models;

namespace VPA.Usecases.DetectionHelpers
{
	public static class ClassHelperUsecase
	{
		public static Dictionary<string, List<ClassNode>> GetClassesPerParentClass(IEnumerable<ClassNode> classNodes)
		{
			var classesPerParentClass = new Dictionary<string, List<ClassNode>>();

			if (!classNodes.Any())
				return classesPerParentClass;

			foreach (var classNode in classNodes)
			{
				if (classNode.ParentClassName == null)
					continue;

				if (!classesPerParentClass.ContainsKey(classNode.ParentClassName))
					classesPerParentClass[classNode.ParentClassName] = new List<ClassNode>();
				
				classesPerParentClass[classNode.ParentClassName].Add(classNode);
			}

			return classesPerParentClass;
		}

		public static Dictionary<string, List<ClassNode>> GetClassesPerInterface(IEnumerable<ClassNode> classNodes)
		{
			var classesPerInterface = new Dictionary<string, List<ClassNode>>();

			if (!classNodes.Any())
				return classesPerInterface;

			foreach (var classNode in classNodes)
			{
				if (classNode.Interfaces == null)
					continue;
				
				foreach (string classInterface in classNode.Interfaces)
				{
					if (!classesPerInterface.ContainsKey(classInterface))
						classesPerInterface[classInterface] = new List<ClassNode>();
						
					classesPerInterface[classInterface].Add(classNode);
				}
			}

			return classesPerInterface;
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
