namespace VPA.Configuration.Tests
{
	public class ConfigurationExtensionsTests
	{
		[Fact]
		public void RegisterDetectors_ShouldPass()
		{
			//Arrange
			var testdictionary = new Dictionary<Type, Type>();

			//Act
			testdictionary.RegisterDetectors();

			//Assert
			Assert.NotEmpty(testdictionary);
		}

		[Fact]
		public void RegisterAdapters_ShouldPass()
		{
			//Arrange
			var testdictionary = new Dictionary<Type, Type>();

			//Act
			testdictionary.RegisterAdapters();

			//Assert
			Assert.NotEmpty(testdictionary);
		}
	}
}