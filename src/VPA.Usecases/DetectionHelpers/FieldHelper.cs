using VPA.Domain.Enums;
using VPA.Domain.Models;

namespace VPA.Usecases.DetectionHelpers
{
	public static class FieldHelper
	{
		public static bool ClassHasPublicStaticFieldWithOwnType(ClassNode node, out FieldNode leaf)
		{
			leaf = node.Children.OfType<FieldNode>()
			.Where(n => 
				   n.Modifiers.Contains(ModifiersEnum.Static) 
				&& n.AccessModifier == AccessModifierEnum.Public 
				&& n.Name == node.Name)
			.FirstOrDefault();

			return leaf != null;
		}
	}
}