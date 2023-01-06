using System;
using System.Collections.Generic;
using System.Text;
using VPA.Domain.Models;

namespace VPA.Usecases.Interfaces
{
	public interface IDetectUsecase
	{
		public string PatternName { get; }

		public Task<DetectionResultCollection> Detect(ProjectNode projectNode);
	}
}
