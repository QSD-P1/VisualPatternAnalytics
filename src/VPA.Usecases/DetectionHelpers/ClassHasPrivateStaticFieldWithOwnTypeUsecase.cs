using VPA.Domain.Enums;
using VPA.Domain.Models;
using VPA.Usecases.Interfaces;

namespace VPA.Usecases.DetectionHelpers
{
	public class ClassHasPrivateStaticFieldWithOwnTypeUsecase : IClassHasPrivateStaticFieldWithOwnTypeUsecase
	{
		public bool Execute(ClassNode node, out FieldNode leaf)
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