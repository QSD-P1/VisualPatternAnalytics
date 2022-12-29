using System;
using System.Collections.Generic;
using System.Text;
using VPA.Domain.Models;
using VPA.Usecases.Interfaces;

namespace VPA.Usecases.Detectors
{
	public class DetectCompositeUsecase : IDetectCompositeUsecase
	{
		public string PatternName => "Composite";

		public async Task<DetectorResultCollection> Detect(ProjectNode tree)
		{
			var collection = new DetectorResultCollection()
			{
				Name = PatternName
			};

			return collection;
		}
	}
}
