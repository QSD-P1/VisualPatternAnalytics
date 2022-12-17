using VPA.Domain.Enums;

namespace VPA.Domain.Models
{
	public class FieldNode : BaseNode
	{
		public AccessModifierEnum AccessModifiers { get; set; }
		public string? Type { get; set; }
	}
}
