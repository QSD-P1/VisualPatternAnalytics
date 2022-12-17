using VPA.Domain.Enums;

namespace VPA.Domain.Models
{
	public class ConstructorNode : BaseNode
	{
		public AccessModifierEnum AccessModifiers { get; set; }
		public IEnumerable<string>? Parameter { get; set; }
	}
}
