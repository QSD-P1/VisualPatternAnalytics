namespace VPA.Domain.Models
{
	public class DetectionResult
	{
		public DetectionResult(string name)
		{
			Name = name;
		}
		public string Name { get; set; }
		public IList<DetectedItem> DetectedItems { get; set; } = new List<DetectedItem>();
	}
}
