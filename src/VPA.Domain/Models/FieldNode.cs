namespace VPA.Domain.Models
{
	public class FieldNode : BaseLeaf
	{
		public string? Type { get; set; }

		public override string ObjectTypeName => "Field";
	}
}
