using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPA.Domain.Models;
using VPA.Usecases.DetectionHelpers;

namespace VPA.Usecases.Tests.DetectionHelpers
{
	public class GetClassesWithParentListTypeUsecaseTests
	{
		private readonly GetClassesWithParentListTypeUsecase _getClassesWithParentListType;

		public GetClassesWithParentListTypeUsecaseTests()
		{
			_getClassesWithParentListType = new(
				new GetCollectionGenericObjectUsecase()
			);
		}

		[Fact]
		public void GetClassesWithParentListTypeUsecase_EmptyDict_ReturnEmptyDict()
		{
			var result = _getClassesWithParentListType.Execute(new Dictionary<string, List<ClassNode>>());
			Assert.Empty(result);
		}

		[Fact]
		public void GetClassesWithParentListTypeUsecase_DictWithoutParentListType_ReturnEmptyDict()
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
				{ "Parent", new List<ClassNode> { composite } }
			};

			var result = _getClassesWithParentListType.Execute(withoutParentListTypeDict);
			Assert.Empty(result);
		}

		[Fact]
		public void GetClassesWithParentListTypeUsecase_DictWithParentListType_ReturnClassesWithParentListType()
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
				{ "Parent", new List<ClassNode> { composite } }
			};

			var result = _getClassesWithParentListType.Execute(withoutParentListTypeDict);
			Assert.Equal(
				new Dictionary<string, List<ClassNode>> { { parentName, withoutParentListTypeDict[parentName] } },
				result);
		}
	}
}