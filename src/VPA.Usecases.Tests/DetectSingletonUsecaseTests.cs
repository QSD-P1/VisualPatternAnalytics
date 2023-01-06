using VPA.Domain.Enums;
using VPA.Domain.Models;
using VPA.Usecases.Usecases;

namespace VPA.Usecases.Tests
{
	public class DetectSingletonUsecaseTests
	{
		[Fact]
		public async Task SingletonDetector_DoesNotThrowWhenNothingCanBeDetected()
		{
			var detector = new DetectSingletonUsecase();
			var projectNode = new ProjectNode();

			var exception = await Record.ExceptionAsync(() => detector.Detect(projectNode));

			Assert.Null(exception);
		}

		[Fact]
		public async Task SingletonDetector_DetectsPattern()
		{
			var detector = new DetectSingletonUsecase();

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

			var result = await detector.Detect(projectNode);

			Assert.True(result.Results.Any());
		}

		[Fact]
		public async Task SingletonDetector_DoesNotDetectPattern()
		{
			var detector = new DetectSingletonUsecase();

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

			var result = await detector.Detect(projectNode);

			Assert.True(!result.Results.Any());
		}
	}
}