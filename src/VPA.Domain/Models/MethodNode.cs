using VPA.Domain.Enums;

namespace VPA.Domain.Models
{
	public class MethodNode : BaseNode
	{
		public AccessModifierEnum AccessModifiers { get; set; }
		public string ReturnType { get; set; }
		public IEnumerable<string>? Parameters { get; set; }
	}
}
