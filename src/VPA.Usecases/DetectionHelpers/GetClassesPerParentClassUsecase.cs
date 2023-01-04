using System;
using System.Collections.Generic;
using System.Text;
using VPA.Domain.Models;

namespace VPA.Usecases.DetectionHelpers
{
	public class GetClassesPerParentClassUsecase
	{
		public Dictionary<string, List<ClassNode>> Execute(ProjectNode projectNode)
		{
			var classesPerParentClass = new Dictionary<string, List<ClassNode>>();

			if (projectNode.ClassNodes == null)
				return classesPerParentClass;

			foreach (var classNode in projectNode.ClassNodes)
			{
				if (classNode.ParentClassName == null)
					continue;

				if (!classesPerParentClass.ContainsKey(classNode.ParentClassName))
					classesPerParentClass[classNode.ParentClassName] = new List<ClassNode>();

				classesPerParentClass[classNode.ParentClassName].Add(classNode);
			}

			return classesPerParentClass;
		}
	}
}