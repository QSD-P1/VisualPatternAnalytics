using VPA.Configuration;
using VPA.Domain.Models;
using VPA.Usecases.Interfaces;
using VPA.Usecases.Manager;
using VPA.Usecases.Models;
using VPA.Usecases.Usecases;

namespace VPA.Usecases.Tests
{
	public class PatternManagerUsecaseTests
	{
		[Fact]
		public void UpdateTree_Invokes_DesignPatternsChangedEvent()
		{
			var patternManagerUsecase = new PatternManagerUsecase(new DetectSingletonUsecase());
			var projectNode = new ProjectNode();

			Assert.Raises<DesignPatternsChangedEventArgs>(e => patternManagerUsecase.DesignPatternsChangedEvent += e, e => patternManagerUsecase.DesignPatternsChangedEvent -= e, () => patternManagerUsecase.UpdateTree(projectNode));
		}

		[Fact]
		public void UpdateTree_ReturnsData()
		{
			var patternManagerUsecase = new PatternManagerUsecase(new DetectSingletonUsecase());
			var projectNode = new ProjectNode();
			DesignPatternsChangedEventArgs result = null;
			object eventSender = null;

			// Set result to check
			patternManagerUsecase.DesignPatternsChangedEvent +=
				delegate(object sender, DesignPatternsChangedEventArgs args)
				{
					result = args;
					eventSender = sender;
				};

			// Raise event
			Assert.Raises<DesignPatternsChangedEventArgs>(e => patternManagerUsecase.DesignPatternsChangedEvent += e, e => patternManagerUsecase.DesignPatternsChangedEvent -= e, () => patternManagerUsecase.UpdateTree(projectNode));

			// Check if result is not null and has the correct type
			Assert.NotNull(result);
			Assert.NotNull(result.Result);
			Assert.IsType<List<DetectionResultCollection>>(result.Result);
			Assert.NotNull(eventSender);
			Assert.IsType<PatternManagerUsecase>(eventSender);
		}
	}
}