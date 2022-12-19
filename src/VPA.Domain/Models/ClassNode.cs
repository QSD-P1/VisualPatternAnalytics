namespace VPA.Domain.Models
{
	public class ClassNode : BaseNode
	{
		public IEnumerable<string>? Interfaces { get; set; }
		public string? ParentClassName { get; set; }
	}
}
