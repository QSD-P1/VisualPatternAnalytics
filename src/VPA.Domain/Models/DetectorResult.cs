namespace VPA.Domain.Models
{
	public class DetectorResult
	{
		public DetectorResult()
		{
			Items = new List<DetectedItem>();
		}

		public List<DetectedItem> Items { get; set; }
	}
} 