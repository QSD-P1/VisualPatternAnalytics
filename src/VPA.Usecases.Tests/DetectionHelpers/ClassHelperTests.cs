using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPA.Domain.Models;
using VPA.Usecases.DetectionHelpers;

namespace VPA.Usecases.Tests.DetectionHelpers
{
	public class ClassHelperTests
	{
		[Fact]
		public void GetClassPerParentClass_NoChildren_ReturnEmptyDict()
		{
			var result = ClassHelperUsecase.GetClassesPerParentClass(new ProjectNode());
			Assert.Empty(result);
		}

		[Fact]
		public void GetClassPerParentClass_ChildrenWithoutParent_ReturnEmptyDict()
		{
			var projectNode = new ProjectNode();
			projectNode.ClassNodes = new List<ClassNode>
			{
				new()
				{
					Name = "Test"
				}
			};
			var result = ClassHelperUsecase.GetClassesPerParentClass(projectNode);
			Assert.Empty(result);
		}

		[Fact]
		public void GetClassPerParentClass_ChildrenWithParent_ReturnClassPerParent()
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
			var result = ClassHelperUsecase.GetClassesPerParentClass(projectNode);
			Assert.Equal(new Dictionary<string, List<ClassNode>> {{ classNode.ParentClassName, projectNode.ClassNodes.ToList()} }, result);
		}

		[Fact]
		public void GetClassPerParentClass_TwoChildrenWithParent_ReturnClassPerParent()
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
			var result = ClassHelperUsecase.GetClassesPerParentClass(projectNode);
			Assert.Equal(new Dictionary<string, List<ClassNode>> {{ classNode.ParentClassName, projectNode.ClassNodes.ToList() }}, result);
		}
		
		[Fact]
		public void GetClassesPerInterface_NoChildren_ReturnEmptyDict()
		{
			var result = ClassHelperUsecase.GetClassesPerInterface(new ProjectNode());
			Assert.Empty(result);
		}

		[Fact]
		public void GetClassesPerInterface_ChildrenWithoutParent_ReturnEmptyDict()
		{
			var projectNode = new ProjectNode();
			projectNode.ClassNodes = new List<ClassNode>
			{
				new()
				{
					Name = "Test"
				}
			};
			var result = ClassHelperUsecase.GetClassesPerInterface(projectNode);
			Assert.Empty(result);
		}

		[Fact]
		public void GetClassesPerInterface_ChildrenWithParent_ReturnClassPerParent()
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
			var result = ClassHelperUsecase.GetClassesPerInterface(projectNode);
			Assert.Equal(new Dictionary<string, List<ClassNode>> {{ classNode.Interfaces.First(), projectNode.ClassNodes.ToList()} }, result);
		}

		[Fact]
		public void GetClassesPerInterface_TwoChildrenWithParent_ReturnClassPerParent()
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
			var result = ClassHelperUsecase.GetClassesPerInterface(projectNode);
			Assert.Equal(new Dictionary<string, List<ClassNode>> {{ classNode.Interfaces.First(), projectNode.ClassNodes.ToList() }}, result);
		}
		
		[Fact]
		public void GetClassesWithParentListType_EmptyDict_ReturnEmptyDict()
		{
			var result = ClassHelperUsecase.GetClassesWithParentListType(new Dictionary<string, List<ClassNode>>());
			Assert.Empty(result);
		}

		[Fact]
		public void GetClassesWithParentListType_DictWithoutParentListType_ReturnEmptyDict()
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
				Interfaces = new List<string> { parentName },
				Children = new List<BaseLeaf>
				{
					fieldNode
				}
			};

			var withoutParentListTypeDict = new Dictionary<string, List<ClassNode>>
			{
				{ "Parent", new List<ClassNode> { composite }}
			};

			var result = ClassHelperUsecase.GetClassesWithParentListType(withoutParentListTypeDict);
			Assert.Empty(result);
		}

		[Fact]
		public void GetClassesWithParentListType_DictWithParentListType_ReturnClassesWithParentListType()
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
				Interfaces = new List<string> { parentName },
				Children = new List<BaseLeaf>
				{
					fieldNode
				}
			};

			var withoutParentListTypeDict = new Dictionary<string, List<ClassNode>>
			{
				{ "Parent", new List<ClassNode> { composite }}
			};

			var result = ClassHelperUsecase.GetClassesWithParentListType(withoutParentListTypeDict);
			Assert.Equal(new Dictionary<string, List<ClassNode>>{{ parentName, withoutParentListTypeDict[parentName] }}, result);
		}
	}
}
