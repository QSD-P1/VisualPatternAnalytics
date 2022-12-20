using System;
using System.Threading.Tasks;
using VPA.Domain.Models;
using VPA.Usecases.DetectionHelpers;
using VPA.Usecases.Interfaces;
using VPA.Domain.Enums;
namespace VPA.Usecases.Usecases
{
	public class SingletonDetector : ISingletonDetector
	{
		public SingletonDetector()
		{
		}

		public async Task<object> Detect(ProjectNode project)
		{
			foreach (var classNode in project.ClassNodes)
			{
				AccessModifierHelper.AllTypeOfHasAccessModifier<ConstructorNode>(classNode.ChildNodes, AccessModifierEnum.Private);
			}
			return new object();
		}
	}
}