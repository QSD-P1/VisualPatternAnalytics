using System;
using System.Collections.Generic;
using System.Text;
using static VPA.Domain.Managers.PatternManager;

namespace VPA.Domain.Models
{
	public class Analyzer
	{
		public delegate void AnalyzerDoneEventHandler(List<object> source, EventArgs args);
		public event AnalyzerDoneEventHandler AnalyzerDoneEvent;

		// Placeholder voor logica
		// TODO: Vervangen met een nuttige iets
		public async Task<object> Analyze() {
			return new object();
		}
	}
}
