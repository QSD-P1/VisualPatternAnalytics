using VPA.Domain.Enums;

namespace VPA.Domain.Models
{
	public abstract class BaseLeaf
	{
		public string? Name { get; set; }
		public object? Location { get; set; }
		public IEnumerable<Modifiers>? Modifiers { get; set; }
		public AccessModifierEnum AccessModifier { get; set; }
	}
}
