using System;
using System.Collections.Generic;
using System.Text;
using VPA.Domain.Models;
using VPA.Usecases.Interfaces;

namespace VPA.Usecases.Models
{
	public class DesignPatternsChangedEventArgs : EventArgs
	{
		public List<DetectorResultCollection> result { get; set; }
		public DesignPatternsChangedEventArgs(List<DetectorResultCollection> result) {
			this.result = result;
		}
	}
}
