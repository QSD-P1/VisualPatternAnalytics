using VPA.Domain.Enums;
using VPA.Domain.Models;

namespace VPA.Usecases.Helpers
{
	public static class FieldHelper
	{
		public static bool ClassHasPublicStaticFieldWithOwnType(ClassNode node)
		{
			return node.ChildNodes
				.OfType<FieldNode>()
				.Where(n => n.Modifiers.Contains(ModifiersEnum.Static) && n.AccessModifier == AccessModifierEnum.Public)
				.Any(f => f.Type == node.Name);
		}
	}
}