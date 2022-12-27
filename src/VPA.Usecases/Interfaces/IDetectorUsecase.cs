using System;
using System.Collections.Generic;
using System.Text;
using VPA.Domain.Models;

namespace VPA.Usecases.Interfaces
{
	public interface IDetectorUsecase
	{
		public string PatternName { get; }

		public Task<DetectorResultCollection> Detect(ProjectNode project);
	}
}
