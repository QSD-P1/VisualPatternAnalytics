using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using VPA.Domain.Models;

namespace VPA.Usecases.DetectionHelpers
{
	public static class ClassHelperUsecase
	{
		public static Dictionary<string, List<ClassNode>> GetClassesPerInterface(ProjectNode projectNode)
		{
			var classesPerInterface = new Dictionary<string, List<ClassNode>>();

			if (projectNode.ClassNodes == null)
				return classesPerInterface;

			foreach (var classNode in projectNode.ClassNodes)
			{
				if (classNode.Interfaces == null)
					break;

				foreach (var usedInterface in classNode.Interfaces)
				{
					if (classesPerInterface.ContainsKey(usedInterface))
						classesPerInterface[usedInterface].Add(classNode);

					classesPerInterface.Add(usedInterface, new List<ClassNode> { classNode });
				}
			}

			return classesPerInterface;
		}

		public static Dictionary<string, List<ClassNode>> GetClassesWithInterfaceListType(Dictionary<string, List<ClassNode>> classesPerInterface)
		{
			// Check if class has collection field of interface type
			var classesWithInterfaceListType = new Dictionary<string, List<ClassNode>>();

			foreach (KeyValuePair<string, List<ClassNode>> entry in classesPerInterface)
			{
				var currentInterface = entry.Key;

				foreach (ClassNode classNode in entry.Value)
				{
					if (classNode.Children == null) continue;

					var fields = classNode.Children.OfType<FieldNode>().ToList();

					if (fields.Any()) continue;

					foreach (FieldNode field in fields)
					{
						if (!typeof(IEnumerable<object>).IsAssignableFrom(Type.GetType(field.Type))) continue;

						// Found it!
						if (!classesWithInterfaceListType.ContainsKey(currentInterface))
							classesWithInterfaceListType[currentInterface] = new List<ClassNode>();

						classesWithInterfaceListType[currentInterface].Add(classNode);
					}
				}
			}

			return classesWithInterfaceListType;
		}
	}
}
