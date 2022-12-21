using static VPA.Domain.Managers.PatternManager;

namespace VPA.Domain.Managers.Tests
{
	public class PatternManagerTests
	{
		[Fact]
		public void GetInstance_Returns_PatternManager()
		{
			var instance = PatternManager.GetInstance();
			Assert.NotNull(instance);
			Assert.IsType<PatternManager>(instance);
		}

		[Fact]
		public void DesignPatternsChanged_ShouldSubscribe()
		{
			var instance = PatternManager.GetInstance();
			instance.DesignPatternsChangedEvent += (sender, args) =>
			{
				Assert.Fail("DesignPatternsChanged was invoked but shouldn't be.");
			};
		}

		[Fact]
		public void DesignPatternsChanged_ShouldNotBeInvoked()
		{
			var instance = PatternManager.GetInstance();
			var change = false;
			instance.DesignPatternsChangedEvent += (sender, args) =>
			{
				change = true;
			};
			Assert.False(change);
		}
	}
}