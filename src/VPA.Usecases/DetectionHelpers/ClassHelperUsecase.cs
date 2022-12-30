using System;
using System.Collections.Generic;
using System.Text;
using VPA.Domain.Models;

namespace VPA.Usecases.DetectionHelpers
{
	public static class ClassHelperUsecase
	{
		public static List<string> GetUsedClassInterfaces(ProjectNode projectNode)
		{
			return projectNode.ClassNodes?.SelectMany(cl => cl.Interfaces?.Distinct()).ToList();
		}
	}
}
