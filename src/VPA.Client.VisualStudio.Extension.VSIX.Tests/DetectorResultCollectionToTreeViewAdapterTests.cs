using VPA.Client.VisualStudio.Extension.VSIX.Adapters;

namespace VPA.Client.VisualStudio.Extension.VSIX.Tests
{
	public class DetectorResultCollectionToTreeViewAdapterTests
	{
		//[StaFact]
		//public void DetectorResultAdapter_AdapterReturnsNestedTreeViewItems()
		//{
		//	// Arrange
		//	var adapter = new DetectorResultCollectionToTreeViewToTreeViewAdapter();

		//	var detectorResultCollection = new DetectorResultCollection("Singleton")
		//	{
		//		Results = new List<DetectedItem>()
		//		{
		//			new DetectedItem()
		//			{
		//				MainNode = new ClassNode()
		//				{
		//					Name = "SomeClass"
		//				},
		//				Children = new List<BaseLeaf>()
		//				{
		//					new ConstructorNode(),
		//					new MethodNode(),
		//					new FieldNode(),
		//				}
		//			},
		//		}
		//	};

		//	// Make result after adapter
		//	var constructorItem = new TreeViewItem() { Header = null, Name = null };
		//	var methodItem = new TreeViewItem() { Header = null, Name = null };
		//	var fieldItem = new TreeViewItem() { Header = null, Name = null };

		//	var classItem = new TreeViewItem() { Header = "SomeClass", Name = "SomeClass" };
		//	classItem.Items.Add(constructorItem);
		//	classItem.Items.Add(methodItem);
		//	classItem.Items.Add(fieldItem);

		//	var patternItem = new TreeViewItem() { Header = "Singleton", Name = "Singleton" };
		//	patternItem.Items.Add(classItem);

		//	// Act
		//	var adapted = adapter.Adapt(detectorResultCollection) as TreeViewItem;

		//	// Assert

		//	// Can't use .Equals() so loop through all. Don't ask me why .Equals doesn't work

		//	Assert.Equal(adapted.Name, patternItem.Name);
		//	Assert.Equal(adapted.Header, patternItem.Header);

		//	for (int classIndex = 0; classIndex < adapted.Items.Count; classIndex++)
		//	{
		//		var adaptedClass = adapted.Items[classIndex] as TreeViewItem;
		//		var sourceClass = patternItem.Items[classIndex] as TreeViewItem;

		//		Assert.Equal(adaptedClass.Name, sourceClass.Name);
		//		Assert.Equal(adaptedClass.Header, sourceClass.Header);

		//		for (int nodeIndex = 0; nodeIndex < sourceClass.Items.Count; nodeIndex++)
		//		{
		//			var adaptedNode = adaptedClass.Items[nodeIndex] as TreeViewItem;
		//			var sourceNode = sourceClass.Items[nodeIndex] as TreeViewItem;

		//			Assert.Equal(adaptedNode.Name, sourceNode.Name);
		//			Assert.Equal(adaptedNode.Header, sourceNode.Header);
		//		}
		//	}
		//}

		[Fact]
		public void DetectorResultAdapter_WrongInputThrows()
		{
			// Arrange
			var adapter = new DetectorResultCollectionToTreeViewToTreeViewAdapter();

			// Act
			var result = Record.Exception(() => adapter.Adapt(null));

			// Assert
			Assert.IsType<NullReferenceException>(result);
		}
	}
}