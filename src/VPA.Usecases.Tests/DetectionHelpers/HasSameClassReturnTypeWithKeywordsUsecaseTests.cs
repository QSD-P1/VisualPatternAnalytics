using VPA.Domain.Enums;
using VPA.Domain.Models;
using VPA.Usecases.DetectionHelpers;
using VPA.Usecases.Interfaces;

namespace VPA.Usecases.Tests.DetectionHelpers
{
	public class HasSameClassReturnTypeWithKeywordsUsecaseTests
	{
		private readonly IHasSameClassReturnTypeWithKeywordsUsecase _usecase;

		public HasSameClassReturnTypeWithKeywordsUsecaseTests()
		{
			_usecase = new HasSameClassReturnTypeWithKeywordsUsecase();
		}

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
			var result = _usecase.Execute(classNode, publicStaticKeywords, out var matchedResult);

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
			var result = _usecase.Execute(classNode, publicStaticKeywords, out var matchedResult);

			// Assert
			Assert.Null(matchedResult);
			Assert.False(result);
		}
	}
}