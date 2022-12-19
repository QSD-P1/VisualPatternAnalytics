using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using VPA.Domain.Enums;

namespace VPA.Common.Adapters.Adapters.Roslyn
{
	public static class RoslynExtensionMethods
	{
		private static readonly Dictionary<SyntaxKind, Modifiers> syntaxKindToModifierMap = new()
		{
			{ SyntaxKind.NewKeyword, Modifiers.New },
			{ SyntaxKind.AbstractKeyword, Modifiers.Abstract },
			{ SyntaxKind.SealedKeyword, Modifiers.Sealed },
			{ SyntaxKind.StaticKeyword, Modifiers.Static },
			{ SyntaxKind.ReadOnlyKeyword, Modifiers.Readonly },
			{ SyntaxKind.VolatileKeyword, Modifiers.Volatile },
			{ SyntaxKind.VirtualKeyword, Modifiers.Virtual },
			{ SyntaxKind.OverrideKeyword, Modifiers.Override },
			{ SyntaxKind.ExternKeyword, Modifiers.Extern },
			{ SyntaxKind.AsyncKeyword, Modifiers.Async },
		};

		public static List<Modifiers> ToModifiers(this SyntaxTokenList syntaxTokenList)
		{
			var methodModifiers = new List<Modifiers>();

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
