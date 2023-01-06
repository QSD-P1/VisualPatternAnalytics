namespace VPA.Domain.Models
{
	public class InterfaceNode : BaseNode
	{
		public IEnumerable<string>? Interfaces { get; set; }
		public override string ObjectTypeName => "Interface";

	}
}
