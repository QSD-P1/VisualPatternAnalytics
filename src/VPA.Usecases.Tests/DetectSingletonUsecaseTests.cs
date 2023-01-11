using VPA.Configuration;
using VPA.Domain.Enums;
using VPA.Domain.Models;
using VPA.Usecases.DetectionHelpers;
using VPA.Usecases.Detectors;
using VPA.Usecases.Interfaces;

namespace VPA.Usecases.Tests
{
	public class DetectSingletonUsecaseTests
	{
		private readonly DetectSingletonUsecase _detectSingletonUsecase;

		public DetectSingletonUsecaseTests()
		{
			_detectSingletonUsecase = new DetectSingletonUsecase(
				new AllOfTypeHasAccessModifierUsecase(),
				new ClassHasPrivateStaticFieldWithOwnTypeUsecase(),
				new HasSameClassReturnTypeWithKeywordsUsecase()
				);
		}

		[Fact]
		public async Task SingletonDetector_DoesNotThrowWhenNothingCanBeDetected()
		{
			var projectNode = new ProjectNode();

			var exception = await Record.ExceptionAsync(() => _detectSingletonUsecase.Detect(projectNode));

			Assert.Null(exception);
		}

		[Fact]
		public async Task SingletonDetector_DetectsPattern()
		{
			var className = "TestClass";

			var constructorNode = new ConstructorNode()
			{
				AccessModifier = AccessModifierEnum.Private 
			};

			var fieldNode = new FieldNode()
			{
				Type = className,
				Modifiers = new List<ModifiersEnum> { ModifiersEnum.Static },
				AccessModifier = AccessModifierEnum.Private
			};

			var methodNode = new MethodNode()
			{
				Modifiers = new List<ModifiersEnum> { ModifiersEnum.Static },
				AccessModifier = AccessModifierEnum.Public,
				ReturnType = className
			};

			var projectNode = new ProjectNode()
			{
				ClassNodes = new List<ClassNode>()
				{
					new ClassNode()
					{
						Name = className,
						Children = new List<BaseLeaf>()
						{
							constructorNode,
							fieldNode,
							methodNode
						}
					}
				}
			};

			var result = await _detectSingletonUsecase.Detect(projectNode);

			Assert.True(result.Results.Any());
		}

		[Fact]
		public async Task SingletonDetector_DoesNotDetectPattern()
		{
			var className = "TestClass";

			var constructorNode = new ConstructorNode()
			{
				AccessModifier = AccessModifierEnum.Private
			};

			var fieldNode = new FieldNode()
			{
				Type = className,
				Modifiers = new List<ModifiersEnum> { ModifiersEnum.Static },
				AccessModifier = AccessModifierEnum.Private
			};

			var methodNode = new MethodNode()
			{
				Modifiers = new List<ModifiersEnum> { ModifiersEnum.Static },
				AccessModifier = AccessModifierEnum.Private,
				ReturnType = className
			};

			var projectNode = new ProjectNode()
			{
				ClassNodes = new List<ClassNode>()
				{
					new ClassNode()
					{
						Children = new List<BaseLeaf>()
						{
							constructorNode,
							fieldNode,
							methodNode
						}
					}
				}
			};

			var result = await _detectSingletonUsecase.Detect(projectNode);

			Assert.True(!result.Results.Any());
		}
	}
}