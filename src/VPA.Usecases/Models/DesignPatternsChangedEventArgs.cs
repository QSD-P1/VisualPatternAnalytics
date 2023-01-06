using VPA.Domain.Models;

namespace VPA.Usecases.Models
{
	public class DesignPatternsChangedEventArgs : EventArgs
	{
		public List<DetectionResultCollection> Result { get; set; }
		public DesignPatternsChangedEventArgs(List<DetectionResultCollection> result) {
			this.Result = result;
		}
	}
}
