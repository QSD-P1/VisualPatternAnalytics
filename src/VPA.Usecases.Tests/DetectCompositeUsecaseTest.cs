using VPA.Domain.Models;
using VPA.Usecases.DetectionHelpers;
using VPA.Usecases.Detectors;

namespace VPA.Usecases.Tests
{
	public class DetectCompositeUsecaseTest
	{
		private readonly DetectCompositeUsecase _detectCompositeUsecase;

		public DetectCompositeUsecaseTest()
		{
			_detectCompositeUsecase = new(
				new GetClassesPerParentClassUsecase(),
				new GetClassesPerInterfaceUsecase(),
				new GetClassesWithParentListTypeUsecase(
					new GetCollectionGenericObjectUsecase()
				),
				new GetCollectionGenericObjectUsecase()
			);
		}

		[Fact]
		public async Task CompositeDetector_DoesNotThrowWhenNothingCanBeDetected()
		{
			var projectNode = new ProjectNode();

			var exception = await Record.ExceptionAsync(() => _detectCompositeUsecase.Detect(projectNode));

			Assert.Null(exception);
		}

		[Fact]
		public async Task CompositeDetector_DetectsPatternUsingAbstracts()
		{
			var parentName = "Parent";

			var fieldNode = new FieldNode
			{
				Type = $"List<{parentName}>"
			};

			var composite = new ClassNode
			{
				Name = "Composite",
				ParentClassName = parentName,
				Children = new List<BaseLeaf>
				{
					fieldNode,
				}
			};

			var leaf = new ClassNode
			{
				Name = "Leaf",
				ParentClassName = parentName,
			};

			var projectNode = new ProjectNode
			{
				ClassNodes = new List<ClassNode>
				{
					new()
					{
						Name = parentName,
					},
					composite,
					leaf
				}
			};

			var result = await _detectCompositeUsecase.Detect(projectNode);

			Assert.True(result.Results.Any());
		}

		[Fact]
		public async Task CompositeDetector_DetectsPatternUsingInterfaces()
		{
			var parentName = "Parent";

			var fieldNode = new FieldNode
			{
				Type = $"List<{parentName}>"
			};

			var composite = new ClassNode
			{
				Name = "Composite",
				Interfaces = new List<string> { parentName },
				Children = new List<BaseLeaf>
				{
					fieldNode,
				}
			};

			var leaf = new ClassNode
			{
				Name = "Leaf",
				Interfaces = new List<string> { parentName }
			};

			var projectNode = new ProjectNode
			{
				ClassNodes = new List<ClassNode>
				{
					new()
					{
						Name = parentName,
					},
					composite,
					leaf
				}
			};

			var result = await _detectCompositeUsecase.Detect(projectNode);

			Assert.True(result.Results.Any());
		}

		[Fact]
		public async Task CompositeDetector_NoLeafFound_DoesNotDetectComposite()
		{
			var parentName = "Parent";

			var fieldNode = new FieldNode
			{
				Type = $"List<{parentName}>"
			};

			var composite = new ClassNode
			{
				Name = "Composite",
				ParentClassName = parentName,
				Children = new List<BaseLeaf>
				{
					fieldNode
				}
			};

			var leaf = new ClassNode
			{
				Name = "NotALeaf",
				ParentClassName = parentName,
				Children = new List<BaseLeaf>
				{
					fieldNode
				}
			};

			var projectNode = new ProjectNode
			{
				ClassNodes = new List<ClassNode>
				{
					new()
					{
						Name = parentName,
					},
					composite,
					leaf
				}
			};

			var result = await _detectCompositeUsecase.Detect(projectNode);
			Assert.False(result.Results.Any());
		}

		[Fact]
		public async Task CompositeDetector_NoCompositeFound_DoesNotDetectComposite()
		{
			var parentName = "Parent";

			var fieldNode = new FieldNode
			{
				Type = $"List<{parentName}>"
			};

			var composite = new ClassNode
			{
				Name = "NotAComposite",
				ParentClassName = parentName
			};

			var leaf = new ClassNode
			{
				Name = "Leaf",
				ParentClassName = parentName
			};

			var projectNode = new ProjectNode
			{
				ClassNodes = new List<ClassNode>
				{
					new()
					{
						Name = parentName,
					},
					composite,
					leaf
				}
			};

			var result = await _detectCompositeUsecase.Detect(projectNode);
			Assert.False(result.Results.Any());
		}

		[Fact]
		public async Task CompositeDetector_NotSharingAParent_DoesNotDetectComposite()
		{
			var parentName = "Parent";
			var parentNameTwo = "ParentTwo";

			var fieldNode = new FieldNode
			{
				Type = $"List<{parentName}>"
			};

			var composite = new ClassNode
			{
				Name = "Composite",
				ParentClassName = parentName,
				Children = new List<BaseLeaf>
				{
					fieldNode,
				}
			};

			var leaf = new ClassNode
			{
				Name = "Leaf",
				ParentClassName = parentNameTwo,
			};

			var projectNode = new ProjectNode
			{
				ClassNodes = new List<ClassNode>
				{
					new()
					{
						Name = parentName,
					},
					composite,
					leaf
				}
			};

			var result = await _detectCompositeUsecase.Detect(projectNode);
			Assert.False(result.Results.Any());
		}

		[Fact]
		public async Task CompositeDetector_MissingACollectionOfParent_DoesNotDetectComposite()
		{
			var parentName = "Parent";
			var parentNameTwo = "ParentTwo";

			var fieldNode = new FieldNode
			{
				Type = $"List<{parentNameTwo}>"
			};

			var composite = new ClassNode
			{
				Name = "Composite",
				ParentClassName = parentName,
				Children = new List<BaseLeaf>
				{
					fieldNode,
				}
			};

			var leaf = new ClassNode
			{
				Name = "Leaf",
				ParentClassName = parentName
			};

			var projectNode = new ProjectNode
			{
				ClassNodes = new List<ClassNode>
				{
					new()
					{
						Name = parentName,
					},
					composite,
					leaf
				}
			};

			var result = await _detectCompositeUsecase.Detect(projectNode);
			Assert.False(result.Results.Any());
		}
	}
}