using VPA.Domain.Models;
using VPA.Usecases.Interfaces;
using VPA.Usecases.Models;

namespace VPA.Usecases.Manager
{
	public sealed class ManageDesignPatternDetectionUsecase : IManageDesignPatternDetectionUsecase
	{
		private static List<IDetectUsecase> _detectors = new List<IDetectUsecase>();
		public event EventHandler<DesignPatternsChangedEventArgs> DesignPatternsChangedEvent;

		public ManageDesignPatternDetectionUsecase(
			IDetectSingletonUsecase detectSingletonUsecase, 
			IDetectCompositeUsecase detectCompositeUsecase)
		{
			_detectors.Add(detectSingletonUsecase);
			_detectors.Add(detectCompositeUsecase);
		}

		public async Task UpdateTree(ProjectNode node)
		{
			if (DesignPatternsChangedEvent == null)
				return;

			var result = new List<DetectionResultCollection>();

			foreach (var detector in _detectors)
			{
				var res = await detector.Detect(node);
				result.Add(res);
			}

			DesignPatternsChangedEvent?.Invoke(this, new DesignPatternsChangedEventArgs(result));
		}
	}
}