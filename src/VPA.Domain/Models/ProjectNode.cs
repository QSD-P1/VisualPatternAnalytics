namespace VPA.Domain.Models
{
	public class ProjectNode
	{
		public IEnumerable<ClassNode> ClassNodes { get; set; } = new List<ClassNode>();
		public IEnumerable<InterfaceNode> InterfaceNodes { get; set; } = new List<InterfaceNode>();

	}
}
