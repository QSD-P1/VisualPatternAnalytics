namespace VPA.Domain.Models
{
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
