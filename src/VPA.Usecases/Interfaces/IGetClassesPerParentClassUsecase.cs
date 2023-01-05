using System;
using System.Collections.Generic;
using System.Text;
using VPA.Domain.Models;

namespace VPA.Usecases.Interfaces
{
	public interface IGetClassesPerParentClassUsecase
	{
		public Dictionary<string, List<ClassNode>> Execute(ProjectNode projectNode);
	}
}
