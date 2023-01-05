using VPA.Configuration;
using VPA.Domain.Models;
using VPA.Usecases.Interfaces;
using VPA.Usecases.Manager;
using VPA.Usecases.Models;
using VPA.Usecases.Usecases;
using Moq;

namespace VPA.Usecases.Tests
{
	public class ManageDesignPatternDetectionUsecaseTests
	{
		[Fact]
		public void UpdateTree_Invokes_DesignPatternsChangedEvent()
		{
			var mockSingleton = new Mock<IDetectSingletonUsecase>();
			var mockProjectNode = new Mock<ProjectNode>();
			var manageDesignPatternDetectionUsecase = new ManageDesignPatternDetectionUsecase(mockSingleton.Object);
			Assert.Raises<DesignPatternsChangedEventArgs>(e => manageDesignPatternDetectionUsecase.DesignPatternsChangedEvent += e, e => manageDesignPatternDetectionUsecase.DesignPatternsChangedEvent -= e, () => manageDesignPatternDetectionUsecase.UpdateTree(mockProjectNode.Object));
		}

		[Fact]
		public void UpdateTree_ReturnsData()
		{
			var mockSingleton = new Mock<IDetectSingletonUsecase>();
			var mockProjectNode = new Mock<ProjectNode>();
			var manageDesignPatternDetectionUsecase = new ManageDesignPatternDetectionUsecase(mockSingleton.Object);

			DesignPatternsChangedEventArgs result = null;
			object eventSender = null;

			// Set result to check
			manageDesignPatternDetectionUsecase.DesignPatternsChangedEvent +=
				delegate(object sender, DesignPatternsChangedEventArgs args)
				{
					result = args;
					eventSender = sender;
				};

			// Raise event
			Assert.Raises<DesignPatternsChangedEventArgs>(e => manageDesignPatternDetectionUsecase.DesignPatternsChangedEvent += e, e => manageDesignPatternDetectionUsecase.DesignPatternsChangedEvent -= e, () => manageDesignPatternDetectionUsecase.UpdateTree(mockProjectNode.Object));

			// Check if result is not null and has the correct type
			Assert.NotNull(result);
			Assert.NotNull(result.Result);
			Assert.IsType<List<DetectionResultCollection>>(result.Result);
			Assert.NotNull(eventSender);
			Assert.IsType<ManageDesignPatternDetectionUsecase>(eventSender);
		}
	}
}