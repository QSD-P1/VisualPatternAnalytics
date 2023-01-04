namespace VPA.Domain.Models
{
	public class DetectionResultCollection
	{
		public DetectionResultCollection(string detectedPatternName)
		{
			DetectedPatternName = detectedPatternName;
		}

		public string DetectedPatternName { get; set; }

		public List<DetectionResult> Results { get; set; } = new();
	}
}
