using VPA.Domain.Models;
using VPA.Usecases.Interfaces;
using VPA.Usecases.Models;

namespace VPA.Usecases.Manager
{
	public sealed class PatternManagerUsecase : IPatternManagerUsecase
	{
		private static List<IDetectorUsecase> _detectors = new List<IDetectorUsecase>();
		public event EventHandler<DesignPatternsChangedEventArgs> DesignPatternsChangedEvent;

		public PatternManagerUsecase(ISingletonDetectorUsecase singletonDetector)
		{
			_detectors.Add(singletonDetector);
		}

		public async Task UpdateTree(ProjectNode node)
		{
			var result = new List<DetectorResultCollection>();

			foreach (var detector in _detectors)
			{
				var res = await detector.Detect(node);
				result.Add(res);
			}

			DesignPatternsChangedEvent.Invoke(this, new DesignPatternsChangedEventArgs(result));
		}
	}
}
