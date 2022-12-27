using VPA.Domain.Models;
using VPA.Usecases.DetectionHelpers;
using VPA.Domain.Enums;

namespace VPA.Usecases.Tests.DetectionHelpers
{
	public class AccessModifierHelperTests
	{
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
			var result = AccessModifierHelper.AllOfTypeHasAccessModifier<ConstructorNode>(classNode.Children, AccessModifierEnum.Private, out var matchedLeaves);

			// Assert
			Assert.True(matchedLeaves.Any());
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
			var result = AccessModifierHelper.AllOfTypeHasAccessModifier<ConstructorNode>(classNode.Children, AccessModifierEnum.Private, out var matchedLeaves);

			// Assert
			Assert.Null(matchedLeaves);
			Assert.False(result);
		}
	}
}