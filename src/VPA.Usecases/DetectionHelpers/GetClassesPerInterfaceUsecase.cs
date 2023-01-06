using VPA.Domain.Models;
using VPA.Usecases.Interfaces;

namespace VPA.Usecases.DetectionHelpers
{
	public class GetClassesPerInterfaceUsecase : IGetClassesPerInterfaceUsecase
	{
		public Dictionary<string, List<ClassNode>> Execute(ProjectNode projectNode)
		{
			var classesPerInterface = new Dictionary<string, List<ClassNode>>();

			if (projectNode.ClassNodes == null)
				return classesPerInterface;

			foreach (var classNode in projectNode.ClassNodes)
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
	}
}