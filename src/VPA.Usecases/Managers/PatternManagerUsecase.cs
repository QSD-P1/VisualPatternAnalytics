using VPA.Domain.Models;
using VPA.Usecases.Interfaces;
using VPA.Usecases.Models;

namespace VPA.Usecases.Manager
{
	public sealed class PatternManagerUsecase : IPatternManagerUsecase
	{
		private static List<IDetectUsecase> _detectors = new List<IDetectUsecase>();
		public event EventHandler<DesignPatternsChangedEventArgs> DesignPatternsChangedEvent;

		public PatternManagerUsecase(IDetectSingletonUsecase detectSingletonUsecase)
		{
			_detectors.Add(detectSingletonUsecase);
		}

		public async Task UpdateTree(ProjectNode node)
		{
			var result = new List<DetectionResultCollection>();

			foreach (var detector in _detectors)
			{
				var res = await detector.Detect(node);
				result.Add(res);
			}

			DesignPatternsChangedEvent.Invoke(this, new DesignPatternsChangedEventArgs(result));
		}
	}
}
