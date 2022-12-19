using Microsoft.CodeAnalysis.CSharp;
using VPA.Common.Adapters.Adapters.Roslyn;
using VPA.Domain.Enums;

namespace VPA.Common.Adapters.Tests
{
	public class RoslynExtensionMethodsTests
	{
		[Theory]
		[InlineData(SyntaxKind.NewKeyword, Modifiers.New)]
		[InlineData(SyntaxKind.AbstractKeyword, Modifiers.Abstract)]
		[InlineData(SyntaxKind.SealedKeyword, Modifiers.Sealed)]
		[InlineData(SyntaxKind.StaticKeyword, Modifiers.Static)]
		[InlineData(SyntaxKind.ReadOnlyKeyword, Modifiers.Readonly)]
		[InlineData(SyntaxKind.VolatileKeyword, Modifiers.Volatile)]
		[InlineData(SyntaxKind.VirtualKeyword, Modifiers.Virtual)]
		[InlineData(SyntaxKind.OverrideKeyword, Modifiers.Override)]
		[InlineData(SyntaxKind.ExternKeyword, Modifiers.Extern)]
		[InlineData(SyntaxKind.AsyncKeyword, Modifiers.Async)]
		public void ToModifiers_ReturnsCorrectModifier(SyntaxKind input, Modifiers expectedOutput)
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
