using System;
using System.Collections.Generic;
using System.Text;
using VPA.Domain.Models;

namespace VPA.Domain.Managers
{
	public sealed class PatternManager
	{
		private static PatternManager _instance;
		private static List<Analyzer> _enabledAnalyzers = new List<Analyzer>();
		private static List<Analyzer> _analyzersInProgress = new List<Analyzer>();
		public List<object> DetectedPatterns = new List<object>();
		public event DesignPatternsChangedEventHandler DesignPatternsChangedEvent;
		public delegate void DesignPatternsChangedEventHandler(Analyzer sender, List<object> location);

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

		public async static void ExecuteAnalyzers()
		{
			foreach (var analyzer in _enabledAnalyzers)
			{
				analyzer.AnalyzerDoneEvent += _instance.OnAnalyzerDone;
				_analyzersInProgress.Add(analyzer);
				await analyzer.Analyze();
			}
		}

		public void OnAnalyzerDone(Analyzer analyzer, List<object> location)
		{
			_analyzersInProgress.Remove(analyzer);

			// Check if there are any new design pattern detections in the list
			if (analyzer.locations.Except(_instance.DetectedPatterns).Any())
			{
				_instance.DesignPatternsChangedEvent?.Invoke(analyzer, location);
			}
		}
	}
}
