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

	public class DetectorResult
	{
		public DetectorResult()
		{
			Items = new List<DetectedItem>();
		}

		public List<DetectedItem> Items { get; set; }
	}

	public class DetectedItem
	{
		public DetectedItem()
		{
			Children = new List<BaseLeaf>();
		}

		public BaseNode MainNode { get; set; }

		public List<BaseLeaf> Children { get; set; }
	}
}
