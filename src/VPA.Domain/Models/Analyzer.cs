using System;
using System.Collections.Generic;
using System.Text;
using static VPA.Domain.Managers.PatternManager;

namespace VPA.Domain.Models
{
	public class Analyzer
	{
		public delegate void AnalyzerDoneEventHandler(Analyzer analyzer, List<object> locations);
		public event AnalyzerDoneEventHandler AnalyzerDoneEvent;
		public List<object> locations = new List<object>();

		// Placeholder voor logica
		// TODO: Vervangen met een nuttige iets
		public async Task<object> Analyze(ProjectNode node) {
			return new object();
		}
	}
}
