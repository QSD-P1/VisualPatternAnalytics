namespace VPA.Domain.Models
{
	public class DetectorResultCollection
	{
		public DetectorResultCollection(string name)
		{
			Name = name;
			Results = new List<DetectedItem>();
		}

		public string Name { get; set; }

		public List<DetectedItem> Results { get; set; }
	}
}
