using VPA.Domain.Models;
using VPA.Usecases.Interfaces;

namespace VPA.Usecases.Manager
{
	public sealed class PatternManagerUsecase
	{
		private static PatternManagerUsecase _instance;
		private static List<IDetectUsecase> _detectors = new List<IDetectUsecase>();
		public event DesignPatternsChangedEventHandler DesignPatternsChangedEvent;
		public delegate void DesignPatternsChangedEventHandler(PatternManagerUsecase sender, List<DetectorResultCollection> results);

		private PatternManagerUsecase()
		{
		}

		public static PatternManagerUsecase GetInstance()
		{
			if (_instance == null)
			{
				_instance = new PatternManagerUsecase();
			}
			return _instance;
		}

		public async Task UpdateTree(ProjectNode node)
		{
			var result = new List<DetectorResultCollection>();

			foreach (var detector in _detectors)
			{
				var res = await detector.Detect(node);
				result.Add(res);
			}

			DesignPatternsChangedEvent.Invoke(this, result);
		}
	}
}
