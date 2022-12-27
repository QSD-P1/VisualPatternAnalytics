using VPA.Domain.Enums;

namespace VPA.Domain.Models
{
	public class ConstructorNode : BaseNode
	{
		//Override 2 properties since only the constructorNode does not implement these two
		private new IEnumerable<ModifiersEnum>? Modifiers;

		public IEnumerable<string>? Parameter { get; set; }
	}
}
