using System;
using System.Collections.Generic;
using System.Text;
using VPA.Domain.Models;

namespace VPA.Usecases.Interfaces
{
	public interface IDetector
	{
		public string PatternName { get; }

		public Task<DetectorResultCollection> Detect(ProjectNode tree);
	}
}
