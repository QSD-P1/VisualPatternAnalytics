using VPA.Usecases.DetectionHelpers;

namespace VPA.Usecases.Tests.DetectionHelpers
{
	public class GetCollectionGenericObjectUsecaseTests
	{
		private readonly GetCollectionGenericObjectUsecase _getCollectionGenericObject;

		public GetCollectionGenericObjectUsecaseTests()
		{
			_getCollectionGenericObject = new();
		}
		
		[Fact]
		public void GetCollectionGenericObject_WithGeneric_ReturnCollectionGenericObject()
		{
			var genericCollectionString = "List<Parent>";
			var result = _getCollectionGenericObject.Execute(genericCollectionString);
			Assert.Equal(result.CollectionType, "List");
			Assert.Equal(result.GenericType, "Parent");
		}

		[Fact]
		public void GetCollectionGenericObject_WithoutGeneric_DontReturnCollectionGenericObject()
		{
			var genericCollectionString = "List";
			var result = _getCollectionGenericObject.Execute(genericCollectionString);
			Assert.Equal(result.CollectionType, "List");
			Assert.Equal(result.GenericType, null);
		}

		[Fact]
		public void GetCollectionGenericObject_WithNullParam_ReturnsNull()
		{
			var result = _getCollectionGenericObject.Execute(null);
			Assert.Null(result);
		}

		[Fact]
		public void GetCollectionGenericObject_WithEmptyString_ReturnsNull()
		{
			var result = _getCollectionGenericObject.Execute("");
			Assert.Null(result);
		}
	}
}
