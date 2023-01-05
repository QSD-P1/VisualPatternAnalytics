using System.Collections.Generic;
using VPA.Domain.Enums;
using VPA.Domain.Models;

namespace VPA.Usecases.DetectionHelpers
{
	public static class FieldHelper
	{
		public static bool ClassHasPrivateStaticFieldWithOwnType(ClassNode node, out FieldNode leaf)
		{
			leaf = node.Children.OfType<FieldNode>()
			.Where(n => 
				   n.Modifiers.Contains(ModifiersEnum.Static) 
				&& n.AccessModifier == AccessModifierEnum.Private 
				&& n.Type?.Replace("?", "") == node.Name)
			.FirstOrDefault();

			return leaf != null;
		}
	}
}