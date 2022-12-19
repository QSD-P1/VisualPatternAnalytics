using Microsoft.CodeAnalysis.CSharp;
using VPA.Common.Adapters.Adapters.Roslyn;
using VPA.Domain.Enums;

namespace VPA.Common.Adapters.Tests
{
	public class RoslynExtensionMethodsTests
	{
		[Theory]
		[InlineData(SyntaxKind.NewKeyword, ModifiersEnum.New)]
		[InlineData(SyntaxKind.AbstractKeyword, ModifiersEnum.Abstract)]
		[InlineData(SyntaxKind.SealedKeyword, ModifiersEnum.Sealed)]
		[InlineData(SyntaxKind.StaticKeyword, ModifiersEnum.Static)]
		[InlineData(SyntaxKind.ReadOnlyKeyword, ModifiersEnum.Readonly)]
		[InlineData(SyntaxKind.VolatileKeyword, ModifiersEnum.Volatile)]
		[InlineData(SyntaxKind.VirtualKeyword, ModifiersEnum.Virtual)]
		[InlineData(SyntaxKind.OverrideKeyword, ModifiersEnum.Override)]
		[InlineData(SyntaxKind.ExternKeyword, ModifiersEnum.Extern)]
		[InlineData(SyntaxKind.AsyncKeyword, ModifiersEnum.Async)]
		public void ToModifiers_ReturnsCorrectModifier(SyntaxKind input, ModifiersEnum expectedOutput)
		{
			// Arrange
			var syntaxTokenList = SyntaxFactory.TokenList(SyntaxFactory.Token(input));

			// Act
			var result = syntaxTokenList.ToModifiers();

			// Assert
			Assert.Single(result);
			Assert.Equal(expectedOutput, result.Single());
		}

	}
}
