namespace VPA.Domain.Models
{
	public abstract class BaseNode : BaseLeaf
	{
		public IEnumerable<BaseLeaf>? ChildNodes { get; set; }
	}
}
