namespace VPA.Domain.Models
{
	public class DetectorResultCollection
	{
		public DetectorResultCollection()
		{
			Results = new List<DetectorResult>();
		}

		public string Name { get; set; }

		public List<DetectorResult> Results { get; set; }
	}
}
