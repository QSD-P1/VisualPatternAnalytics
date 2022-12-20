using System;
using System.Collections.Generic;
using System.Text;
using VPA.Domain.Models;

namespace VPA.Usecases.Interfaces
{
	public interface IDetector
	{
		public Task<object> Detect(ProjectNode tree);
	}
}
