namespace VPA.Domain.Models
{
	public class MethodNode : BaseLeaf
	{
		public string? ReturnType { get; set; }
		public IEnumerable<string>? Parameters { get; set; }

		public override string ObjectTypeName => "Method";
	}
}
