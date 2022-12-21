using VPA.Configuration.Tests.Models;
using VPA.Usecases.Interfaces;

namespace VPA.Configuration.Tests
{
	public class DefaultConfigurationTests
	{
		[Fact]
		public void GetInstace_ShouldGiveInstance()
		{
			//act
			var result = DefaultConfiguration.GetInstance();

			//assert
			Assert.NotNull(result);
		}

		[Fact]
		public void GetInstace_ShouldBeSameInstance()
		{
			//act
			var result1 = DefaultConfiguration.GetInstance();
			var result2 = DefaultConfiguration.GetInstance();

			//assert
			Assert.Same(result1, result2);
		}

		[Fact]
		public void GetService_ShouldReturnAnalyzeSingleton()
		{
			//arrange
			var config = DefaultConfiguration.GetInstance();

			//act

			//Huge assumption that we inject the `IAnalyzeSingletonUsecase`.
			//Feel free to change the interface if we dont use this interface.
			var result = config.GetService<IDetectSingletonUsecase>();

			//assert
			Assert.Multiple(
				() => Assert.NotNull(result),
				() => Assert.IsAssignableFrom<IDetectSingletonUsecase>(result)
				);
		}

		[Fact]
		public void GetService_ShouldThrowException() 
		{
			//arrange
			var config = DefaultConfiguration.GetInstance();

			//act & assert
			Assert.Throws<ArgumentException>(config.GetService<IInvalidInterfaceForConfiguration>);
				
		}
	}
}
