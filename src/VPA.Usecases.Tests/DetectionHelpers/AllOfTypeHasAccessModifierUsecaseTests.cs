using VPA.Domain.Models;
using VPA.Usecases.DetectionHelpers;
using VPA.Domain.Enums;
using VPA.Usecases.Interfaces;

namespace VPA.Usecases.Tests.DetectionHelpers
{
	public class AllOfTypeHasAccessModifierUsecaseTests
	{
		private readonly IAllOfTypeHasAccessModifierUsecase _usecase;

		public AllOfTypeHasAccessModifierUsecaseTests()
		{
			_usecase = new AllOfTypeHasAccessModifierUsecase();
		}

		[Fact]
		public void AllOfTypeHasAccessModifier_WhenUsingPrivateConstructorsOnly_ShouldReturnTrue()
		{
			// Arrange
			var constructorNode = new ConstructorNode()
			{
				AccessModifier = AccessModifierEnum.Private,
			};
			var methodNode = new MethodNode()
			{
				AccessModifier = AccessModifierEnum.Public,
			};
			var classNode = new ClassNode()
			{
				Children = new List<BaseLeaf>
				{
					constructorNode,
					constructorNode,
					constructorNode,
					constructorNode,
					methodNode,
					methodNode
				}
			};

			// Act
			var result = _usecase.Execute<ConstructorNode>(classNode.Children, AccessModifierEnum.Private, out var matchedLeaves);

			// Assert
			Assert.True(matchedLeaves != null && matchedLeaves.Any());
			Assert.True(result);
		}

		[Fact]
		public void AllOfTypeHasAccessModifier_WhenNotUsingPrivateConstructorsOnly_ShouldReturnFalse()
		{
			// Arrange
			var privateConstructorNode = new ConstructorNode()
			{
				AccessModifier = AccessModifierEnum.Private,
			};
			var publicConstructorNode = new ConstructorNode()
			{
				AccessModifier = AccessModifierEnum.Public,
			};
			var classNode = new ClassNode()
			{
				Children = new List<BaseLeaf>
				{
					privateConstructorNode,
					privateConstructorNode,
					privateConstructorNode,
					privateConstructorNode,
					publicConstructorNode,
					publicConstructorNode
				}
			};

			// Act
			var result = _usecase.Execute<ConstructorNode>(classNode.Children, AccessModifierEnum.Private, out var matchedLeaves);

			// Assert
			Assert.Null(matchedLeaves);
			Assert.False(result);
		}
	}
}