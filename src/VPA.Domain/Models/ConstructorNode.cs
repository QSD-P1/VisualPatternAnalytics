using VPA.Domain.Enums;

namespace VPA.Domain.Models
{
	public class ConstructorNode : BaseNode
	{
		//Override 2 properties since only the constructorNode does not implement these two
		public new string Name = "Constructor";
		public new IEnumerable<Modifiers>? Modifiers = null;

		public IEnumerable<string>? Parameter { get; set; }
	}
}
