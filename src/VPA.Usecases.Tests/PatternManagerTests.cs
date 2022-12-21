using VPA.Usecases.Manager;

namespace VPA.Usecases.Tests
{
	public class PatternManagerTests
	{
		[Fact]
		public void GetInstance_Returns_PatternManager()
		{
			var instance = PatternManagerUsecase.GetInstance();
			Assert.NotNull(instance);
			Assert.IsType<PatternManagerUsecase>(instance);
		}
	}
}