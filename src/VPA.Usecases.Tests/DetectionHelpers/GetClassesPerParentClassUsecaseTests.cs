using VPA.Domain.Models;
using VPA.Usecases.DetectionHelpers;

namespace VPA.Usecases.Tests.DetectionHelpers
{
	public class GetClassesPerParentClassUsecaseTests
	{
		private readonly GetClassesPerParentClassUsecase _getClassesPerParentClass;

		public GetClassesPerParentClassUsecaseTests()
		{
			_getClassesPerParentClass = new();
		}

		[Fact]
		public void GetClassesPerParentClassUsecase_NoChildren_ReturnEmptyDict()
		{
			var result = _getClassesPerParentClass.Execute(new ProjectNode());
			Assert.Empty(result);
		}

		[Fact]
		public void GetClassesPerParentClassUsecase_ChildrenWithoutParent_ReturnEmptyDict()
		{
			var projectNode = new ProjectNode();
			projectNode.ClassNodes = new List<ClassNode>
			{
				new()
				{
					Name = "Test"
				}
			};
			var result = _getClassesPerParentClass.Execute(projectNode);
			Assert.Empty(result);
		}

		[Fact]
		public void GetClassesPerParentClassUsecase_ChildrenWithParent_ReturnClassPerParent()
		{
			var projectNode = new ProjectNode();
			var classNode = new ClassNode()
			{
				Name = "Test",
				ParentClassName = "TestParent"
			};

			projectNode.ClassNodes = new List<ClassNode>
			{
				classNode
			};
			var result = _getClassesPerParentClass.Execute(projectNode);
			Assert.Equal(new Dictionary<string, List<ClassNode>> {{ classNode.ParentClassName, projectNode.ClassNodes.ToList()} }, result);
		}

		[Fact]
		public void GetClassesPerParentClassUsecase_TwoChildrenWithParent_ReturnClassPerParent()
		{
			var projectNode = new ProjectNode();
			var classNode = new ClassNode()
			{
				Name = "Test",
				ParentClassName = "TestParent"
			};

			var classNodeTwo = new ClassNode()
			{
				Name = "TestTwo",
				ParentClassName = "TestParent"
			};

			projectNode.ClassNodes = new List<ClassNode>
			{
				classNode,
				classNodeTwo
			};
			var result = _getClassesPerParentClass.Execute(projectNode);
			Assert.Equal(new Dictionary<string, List<ClassNode>> {{ classNode.ParentClassName, projectNode.ClassNodes.ToList() }}, result);
		}
	}
}
