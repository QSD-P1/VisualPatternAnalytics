using VPA.Domain.Models;
using VPA.Usecases.DetectionHelpers;
using VPA.Domain.Enums;

namespace VPA.Usecases.Tests
{
	public class SingletonDetectorTests
	{
		[Fact]
		public void AllTypeOfHasAccessModifier_WhenUsingPrivateConstructorsOnly_ShouldReturnTrue()
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
				ChildNodes = new List<BaseLeaf>
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
			var result = AccessModifierHelper.AllTypeOfHasAccessModifier<ConstructorNode>(classNode.ChildNodes, AccessModifierEnum.Private);

			// Assert
			Assert.True(result);
		}

		[Fact]
		public void AllTypeOfHasAccessModifier_WhenNotUsingPrivateConstructorsOnly_ShouldReturnFalse()
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
				ChildNodes = new List<BaseLeaf>
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
			var result = AccessModifierHelper.AllTypeOfHasAccessModifier<ConstructorNode>(classNode.ChildNodes, AccessModifierEnum.Private);

			// Assert
			Assert.False(result);
		}
	}
}