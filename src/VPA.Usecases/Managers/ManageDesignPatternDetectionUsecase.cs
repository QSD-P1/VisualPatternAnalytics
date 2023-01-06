using VPA.Domain.Models;
using VPA.Usecases.Interfaces;
using VPA.Usecases.Models;

namespace VPA.Usecases.Manager
{
	public sealed class ManageDesignPatternDetectionUsecase : IManageDesignPatternDetectionUsecase
	{
		private static List<IDetectUsecase> _detectors = new List<IDetectUsecase>();
		public event EventHandler<DesignPatternsChangedEventArgs> DesignPatternsChangedEvent;

		public ManageDesignPatternDetectionUsecase(IDetectSingletonUsecase detectSingletonUsecase, IDetectProxyUsecase detectProxyUsecase)
		{
			_detectors.Add(detectSingletonUsecase);
			_detectors.Add(detectProxyUsecase);
		}

		public async Task UpdateTree(ProjectNode projectNode)
		{
			if (DesignPatternsChangedEvent == null)
				return;

			var result = new List<DetectionResultCollection>();

			if (projectNode.ClassNodes != null && projectNode.ClassNodes.Any(c => c.Children != null && c.Children.Any()))
			{
				foreach (var detector in _detectors)
				{
					var res = await detector.Detect(projectNode);
					result.Add(res);
				}
			}

			DesignPatternsChangedEvent.Invoke(this, new DesignPatternsChangedEventArgs(result));
		}
	}
}
