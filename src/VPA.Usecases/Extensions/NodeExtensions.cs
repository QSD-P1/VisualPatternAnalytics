using System.Collections;
using VPA.Domain.Enums;
using VPA.Domain.Models;

namespace VPA.Usecases.Extensions
{
	public static class NodeExtensions
	{
		public static IEnumerable<T> OfTypeWithAccessModifier<T>(this IEnumerable source, AccessModifierEnum accessModifier) where T : BaseLeaf
		{
			return source.OfType<T>().Where(x => x.AccessModifier == accessModifier);
		}

		public static IEnumerable<T> OfTypeWithModifiers<T>(this IEnumerable source, IEnumerable<ModifiersEnum> modifiers) where T : BaseLeaf
		{
			return source.OfType<T>().Where(x => x.Modifiers != null && modifiers.All(m => x.Modifiers.Contains(m)));
		}
	}
}
