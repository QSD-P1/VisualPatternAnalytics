using VPA.Domain.Models;
using VPA.Usecases.DetectionHelpers;

namespace VPA.Usecases.Tests.DetectionHelpers
{
	public class GetClassesPerInterfaceUsecaseTests
	{
		private readonly GetClassesPerInterfaceUsecase _getClassesPerInterface;

		public GetClassesPerInterfaceUsecaseTests()
		{
			_getClassesPerInterface = new();
		}
		
		[Fact]
		public void GetClassesPerInterfaceUsecase_NoChildren_ReturnEmptyDict()
		{
			var result = _getClassesPerInterface.Execute(new ProjectNode());
			Assert.Empty(result);
		}

		[Fact]
		public void GetClassesPerInterfaceUsecase_ChildrenWithoutParent_ReturnEmptyDict()
		{
			var projectNode = new ProjectNode();
			projectNode.ClassNodes = new List<ClassNode>
			{
				new()
				{
					Name = "Test"
				}
			};
			var result = _getClassesPerInterface.Execute(projectNode);
			Assert.Empty(result);
		}

		[Fact]
		public void GetClassesPerInterfaceUsecase_ChildrenWithParent_ReturnClassPerParent()
		{
			var projectNode = new ProjectNode();
			var classNode = new ClassNode()
			{
				Name = "Test",
				Interfaces = new List<string>() { "TestInterface" }
			};

			projectNode.ClassNodes = new List<ClassNode>
			{
				classNode
			};
			var result = _getClassesPerInterface.Execute(projectNode);
			Assert.Equal(new Dictionary<string, List<ClassNode>> {{ classNode.Interfaces.First(), projectNode.ClassNodes.ToList()} }, result);
		}

		[Fact]
		public void GetClassesPerInterfaceUsecase_TwoChildrenWithParent_ReturnClassPerParent()
		{
			var projectNode = new ProjectNode();
			var classNode = new ClassNode()
			{
				Name = "Test",
				Interfaces = new List<string>() { "TestInterface" }
			};

			var classNodeTwo = new ClassNode()
			{
				Name = "TestTwo",
				Interfaces = new List<string>() { "TestInterface" }
			};

			projectNode.ClassNodes = new List<ClassNode>
			{
				classNode,
				classNodeTwo
			};
			var result = _getClassesPerInterface.Execute(projectNode);
			Assert.Equal(new Dictionary<string, List<ClassNode>> {{ classNode.Interfaces.First(), projectNode.ClassNodes.ToList() }}, result);
		}
	}
}
