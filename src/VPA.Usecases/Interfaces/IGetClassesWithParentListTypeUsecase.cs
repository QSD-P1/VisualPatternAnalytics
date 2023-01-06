using System;
using System.Collections.Generic;
using System.Text;
using VPA.Domain.Models;

namespace VPA.Usecases.Interfaces
{
	public interface IGetClassesWithParentListTypeUsecase
	{
		public Dictionary<string, List<ClassNode>> Execute(Dictionary<string, List<ClassNode>> classesPerParent);
	}
}
