using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using VPA.Domain.Enums;

namespace VPA.Common.Adapters.Adapters.Roslyn
{
	public static class RoslynExtensionMethods
	{
		private static readonly Dictionary<SyntaxKind, ModifiersEnum> syntaxKindToModifierMap = new()
		{
			{ SyntaxKind.NewKeyword, ModifiersEnum.New },
			{ SyntaxKind.AbstractKeyword, ModifiersEnum.Abstract },
			{ SyntaxKind.SealedKeyword, ModifiersEnum.Sealed },
			{ SyntaxKind.StaticKeyword, ModifiersEnum.Static },
			{ SyntaxKind.ReadOnlyKeyword, ModifiersEnum.Readonly },
			{ SyntaxKind.VolatileKeyword, ModifiersEnum.Volatile },
			{ SyntaxKind.VirtualKeyword, ModifiersEnum.Virtual },
			{ SyntaxKind.OverrideKeyword, ModifiersEnum.Override },
			{ SyntaxKind.ExternKeyword, ModifiersEnum.Extern },
			{ SyntaxKind.AsyncKeyword, ModifiersEnum.Async },
		};

		public static List<ModifiersEnum> ToModifiers(this SyntaxTokenList syntaxTokenList)
		{
			var methodModifiers = new List<ModifiersEnum>();

			//Iterate over the SyntaxTokenList
			foreach (var syntaxToken in syntaxTokenList)
			{
				//Check if the SyntaxKind of the SyntaxToken is present in the dictionary
				if (syntaxKindToModifierMap.TryGetValue(syntaxToken.Kind(), out var modifier))
				{
					//If the SyntaxKind is present in the dictionary, add the corresponding Modifiers value to the list
					methodModifiers.Add(modifier);
				}
			}

			return methodModifiers;
		}
	}
}
