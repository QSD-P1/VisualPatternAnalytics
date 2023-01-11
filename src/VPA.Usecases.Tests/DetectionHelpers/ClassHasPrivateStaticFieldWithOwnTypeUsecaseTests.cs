using VPA.Domain.Enums;
using VPA.Domain.Models;
using VPA.Usecases.DetectionHelpers;
using VPA.Usecases.Interfaces;

namespace VPA.Usecases.Tests.DetectionHelpers
{
	public class ClassHasPrivateStaticFieldWithOwnTypeUsecaseTests
	{
		private readonly IClassHasPrivateStaticFieldWithOwnTypeUsecase _usecase;

		public ClassHasPrivateStaticFieldWithOwnTypeUsecaseTests()
		{
			_usecase = new ClassHasPrivateStaticFieldWithOwnTypeUsecase();
		}

		[Fact]
		public void ClassHasPublicStaticFieldWithOwnType_ShouldReturnTrue()
		{
			var className = "TestClass";

			var fieldNode = new FieldNode()
			{
				Type = className,
				Modifiers = new List<ModifiersEnum> { ModifiersEnum.Static },
				AccessModifier = AccessModifierEnum.Private
			};

			var classNode = new ClassNode()
			{
				Name = className,
				Children = new List<BaseLeaf>
				{
					fieldNode
				}
			};

			var result = _usecase.Execute(classNode, out var leaf);

			Assert.True(result && leaf != null);
		}

		[Fact]
		public void ClassHasPublicStaticFieldWithOwnType_ShouldReturnFalse()
		{
			var className = "TestClass";

			var fieldNode = new FieldNode()
			{
				Type = className,
				Modifiers = new List<ModifiersEnum> { ModifiersEnum.Static },
				AccessModifier = AccessModifierEnum.Private
			};

			var classNode = new ClassNode()
			{
				Name = className,
				Children = new List<BaseLeaf>
				{
					fieldNode
				}
			};

			var result = _usecase.Execute(classNode, out var leaf);

			Assert.False(result && leaf == null);
		}
	}
}