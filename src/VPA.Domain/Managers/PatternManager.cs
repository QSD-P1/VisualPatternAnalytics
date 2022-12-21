using VPA.Domain.Models;

namespace VPA.Domain.Managers
{
	public sealed class PatternManager
	{
		private static PatternManager _instance;
		private static List<Analyzer> _analyzers = new List<Analyzer>();
		public event DesignPatternsChangedEventHandler DesignPatternsChangedEvent;
		public delegate void DesignPatternsChangedEventHandler(PatternManager sender, List<object> results);

		private PatternManager()
		{
		}

		public static PatternManager GetInstance()
		{
			if (_instance == null)
			{
				_instance = new PatternManager();
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
