using System;
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
			{
				return classesPerInterface;
			}

			foreach (var classNode in projectNode.ClassNodes)
			{
				if (classNode.Interfaces == null)
				{
					break;
				}

				foreach (var usedInterface in classNode.Interfaces)
				{
					if (classesPerInterface.ContainsKey(usedInterface))
					{
						classesPerInterface[usedInterface].Add(classNode);
					}
					else
					{
						classesPerInterface.Add(usedInterface, new List<ClassNode> { classNode });
					}
				}
			}

			return classesPerInterface;
		}
	}
}
