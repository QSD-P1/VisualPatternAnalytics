//using System.Windows.Controls;
//using VPA.Client.VisualStudio.Extension.VSIX.Adapters.Presentation;

using VPA.Domain.Models;

namespace VPA.Common.Adapters.Tests
{
	public class DetectorResultCollectionToTreeViewAdapterTests
	{
		[Fact]
		public void ConvertToGenericTree_ReturnsCorrectClassNode()
		{
			// Arrange
			//var adapter = new DetectorResultCollectionToTreeViewAdapter();

			var detectorResultCollection = new DetectorResultCollection() { 
				Name = "Singleton", 
				Results = new List<DetectorResult>() {
					new	DetectorResult(){
						Items = new List<DetectedItem>()
						{
							new DetectedItem()
							{
								MainNode = new ClassNode()
								{
									Name = "SomeClass.cs"
								},
								Children = new List<BaseLeaf>()
								{
									new ConstructorNode(),
									new MethodNode(),
									new FieldNode(),
								}
							},
						}
					}
				}
			};


			// Act
			//var res = adapter.Adapt(detectorResultCollection);

			// Assert
			
		}

	}
}