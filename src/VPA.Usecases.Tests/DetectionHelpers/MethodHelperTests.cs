using VPA.Domain.Enums;
using VPA.Domain.Models;
using VPA.Usecases.DetectionHelpers;

namespace VPA.Usecases.Tests.DetectionHelpers
{
	public class MethodHelperTests
	{
		[Fact]
		public void HasSameClassReturnTypeAndModifiers_WithValidKeywords_ShouldReturnTrue()
		{
			// Arrange
			var className = "Foo";
			MethodNode methodNode = new MethodNode()
			{
				Modifiers = new [] { ModifiersEnum.Static },
				AccessModifier = AccessModifierEnum.Public,
				ReturnType = className
			};
			ClassNode classNode = new ClassNode()
			{
				Name = className,
				Children = new List<BaseLeaf>()
				{
					methodNode
				}
			};
			var publicStaticKeywords = new KeywordCollection()
			{
				AccessModifier = AccessModifierEnum.Public,
				Modifiers = new[] {
					ModifiersEnum.Static
				}
			};

			// Act
			var result = MethodHelper.HasSameClassReturnTypeWithKeywords(classNode, publicStaticKeywords, out var matchedResult);

			// Assert
			Assert.True(result);
			Assert.NotNull(matchedResult);
		}

		[Fact]
		public void HasSameClassReturnTypeAndModifiers_WithInvalidKeywords_ShouldReturnFalse()
		{
			// Arrange
			var className = "Foo";
			MethodNode methodNode = new MethodNode()
			{
				Modifiers = new [] { ModifiersEnum.Static },
				AccessModifier = AccessModifierEnum.Public,
				ReturnType = className
			};
			ClassNode classNode = new ClassNode()
			{
				Name = className,
				Children = new List<BaseLeaf>()
				{
					methodNode
				}
			};
			var publicStaticKeywords = new KeywordCollection()
			{
				AccessModifier = AccessModifierEnum.Public,
				Modifiers = new[] {
					ModifiersEnum.Static,
					ModifiersEnum.Abstract
				}
			};

			// Act
			var result = MethodHelper.HasSameClassReturnTypeWithKeywords(classNode, publicStaticKeywords, out var matchedResult);

			// Assert
			Assert.Null(matchedResult);
			Assert.False(result);
		}
	}
}
