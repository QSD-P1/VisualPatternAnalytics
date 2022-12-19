using System;
using System.Collections.Generic;
using System.Text;
using VPA.Domain.Models;

namespace VPA.Domain.Managers
{
	public sealed class PatternManager
	{
		private static PatternManager _instance;
		private static List<Analyzer> _analyzers = new List<Analyzer>();
		public List<object> DetectedPatterns = new List<object>();
		public event EventHandler<EventArgs> DesignPatternsChangedEvent;

		public static PatternManager GetInstance()
		{
			if (_instance == null)
			{
				_instance = new PatternManager();
			}
			return _instance;
		}

		public async static void ExecuteAnalyzers()
		{
			foreach (var analyzer in _analyzers)
			{
				analyzer.AnalyzerDoneEvent += _instance.OnAnalyzerDone;
				await analyzer.Analyze();
			}
		}

		public void OnAnalyzerDone(List<object> source, EventArgs eventArgs)
		{
			// Check if there are any new design pattern detections in the list
			if (source.Except(_instance.DetectedPatterns).Any())
			{
				_instance.DesignPatternsChangedEvent?.Invoke(source, eventArgs);
			}
		}
	}
}
