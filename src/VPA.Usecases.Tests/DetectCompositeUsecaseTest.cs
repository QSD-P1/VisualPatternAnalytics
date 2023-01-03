using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPA.Configuration;
using VPA.Domain.Enums;
using VPA.Domain.Models;
using VPA.Usecases.Interfaces;

namespace VPA.Usecases.Tests
{
	public class DetectCompositeUsecaseTest
	{
		[Fact]
		public async Task CompositeDetector_DoesNotThrowWhenNothingCanBeDetected()
		{
			var config = DefaultConfiguration.GetInstance();
			var detector = config.GetService<IDetectCompositeUsecase>();
			var projectNode = new ProjectNode();

			var exception = await Record.ExceptionAsync(() => detector.Detect(projectNode));

			Assert.Null(exception);
		}

		[Fact]
		public async Task CompositeDetector_DetectsPattern()
		{
			var config = DefaultConfiguration.GetInstance();
			var detector = config.GetService<IDetectCompositeUsecase>();

			var parentName = "Parent";

			var fieldNode = new FieldNode
			{
				Type = $"List<{parentName}>"
			};

			var composite = new ClassNode
			{
				Name = "Composite",
				ParentClassName = parentName,
				Interfaces = new List<string> { parentName },
				Children = new List<BaseLeaf>
				{
					fieldNode,
				}
			};

			var leaf = new ClassNode
			{
				Name = "Leaf",
				ParentClassName = parentName,
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

			var result = await detector.Detect(projectNode);

			Assert.True(result.Results.Any());
		}

		[Fact]
		public async Task CompositeDetector_NoLeafFound_DoesNotDetectComposite()
		{
			var config = DefaultConfiguration.GetInstance();
			var detector = config.GetService<IDetectCompositeUsecase>();

			var parentName = "Parent";

			var fieldNode = new FieldNode
			{				
				Type = $"List<{parentName}>"
			};

			var composite = new ClassNode
			{
				Name = "Composite",
				ParentClassName = parentName,
				Interfaces = new List<string> { parentName },
				Children = new List<BaseLeaf>
				{
					fieldNode
				}
			};

			var leaf = new ClassNode
			{
				Name = "NotALeaf",
				ParentClassName = parentName,
				Interfaces = new List<string> { parentName },
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

			var result = await detector.Detect(projectNode);
			Assert.False(result.Results.Any());
		}

		[Fact]
		public async Task CompositeDetector_NoCompositeFound_DoesNotDetectComposite()
		{
			var config = DefaultConfiguration.GetInstance();
			var detector = config.GetService<IDetectCompositeUsecase>();

			var parentName = "Parent";

			var fieldNode = new FieldNode
			{
				Type = $"List<{parentName}>"
			};

			var composite = new ClassNode
			{
				Name = "NotAComposite",
				ParentClassName = parentName,
				Interfaces = new List<string> { parentName }
			};

			var leaf = new ClassNode
			{
				Name = "Leaf",
				ParentClassName = parentName,
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

			var result = await detector.Detect(projectNode);
			Assert.False(result.Results.Any());
		}

		[Fact]
		public async Task CompositeDetector_NotSharingAParent_DoesNotDetectComposite()
		{
			var config = DefaultConfiguration.GetInstance();
			var detector = config.GetService<IDetectCompositeUsecase>();

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
				Interfaces = new List<string> { parentName },
				Children = new List<BaseLeaf>
				{
					fieldNode,
				}
			};

			var leaf = new ClassNode
			{
				Name = "Leaf",
				ParentClassName = parentNameTwo,
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

			var result = await detector.Detect(projectNode);
			Assert.False(result.Results.Any());
		}

		[Fact]
		public async Task CompositeDetector_MissingACollectionOfParent_DoesNotDetectComposite()
		{
			var config = DefaultConfiguration.GetInstance();
			var detector = config.GetService<IDetectCompositeUsecase>();

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
				Interfaces = new List<string> { parentName },
				Children = new List<BaseLeaf>
				{
					fieldNode,
				}
			};

			var leaf = new ClassNode
			{
				Name = "Leaf",
				ParentClassName = parentName,
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

			var result = await detector.Detect(projectNode);
			Assert.False(result.Results.Any());
		}
	}
}
