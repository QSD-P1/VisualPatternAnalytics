using VPA.Usecases;

namespace VPA.Domain.Managers.Tests
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

		[Fact]
		public void DesignPatternsChanged_ShouldSubscribe()
		{
			var instance = PatternManagerUsecase.GetInstance();
			instance.DesignPatternsChangedEvent += (sender, args) =>
			{
				Assert.Fail("DesignPatternsChanged was invoked but shouldn't be.");
			};
		}

		[Fact]
		public void DesignPatternsChanged_ShouldNotBeInvoked()
		{
			var instance = PatternManagerUsecase.GetInstance();
			var change = false;
			instance.DesignPatternsChangedEvent += (sender, args) =>
			{
				change = true;
			};
			Assert.False(change);
		}
	}
}