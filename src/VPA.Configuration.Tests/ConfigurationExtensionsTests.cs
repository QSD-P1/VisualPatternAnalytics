namespace VPA.Configuration.Tests
{
	public class ConfigurationExtensionsTests
	{
		[Fact]
		public void RegisterUsecases_ShouldPass()
		{
			//Arrange
			var testdictionary = new Dictionary<Type, Type>();

			//Act
			testdictionary.RegisterUsecases();

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