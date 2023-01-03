namespace VPA.Domain.Models
{
	public class DetectedItem
	{
		public BaseNode MainNode { get; set; } = default!;

		public List<BaseLeaf> Children { get; set; } = new();
	}
}
