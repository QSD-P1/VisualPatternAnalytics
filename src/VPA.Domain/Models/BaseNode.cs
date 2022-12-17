namespace VPA.Domain.Models
{
	public abstract class BaseNode
	{
		public string? Name { get; set; }
		public object? Location { get; set; }
		public IEnumerable<BaseNode>? ChildNodes { get; set; }
	}
}
