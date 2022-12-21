using VPA.Domain.Models;

namespace VPA.Domain.Managers
{
	public sealed class PatternManagerUsecase
	{
		private static PatternManagerUsecase _instance;
		private static List<Analyzer> _analyzers = new List<Analyzer>();
		public event DesignPatternsChangedEventHandler DesignPatternsChangedEvent;
		public delegate void DesignPatternsChangedEventHandler(PatternManagerUsecase sender, List<object> results);

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
			var result = new List<object>();

			foreach (var analyzer in _analyzers)
			{
				var res = await analyzer.Analyze(node);
				result.Add(res);
			}

			this.DesignPatternsChangedEvent.Invoke(this, result);
		}
	}
}
