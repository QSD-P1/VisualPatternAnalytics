namespace VPA.Domain.Models
{
	public class ProjectNode
	{
		public IEnumerable<ClassNode> ClassNodes { get; set; } = new List<ClassNode>();
	}
}
