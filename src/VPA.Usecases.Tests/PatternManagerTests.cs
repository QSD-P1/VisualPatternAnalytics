using VPA.Configuration;
using VPA.Domain.Models;
using VPA.Usecases.Interfaces;
using VPA.Usecases.Manager;
using VPA.Usecases.Models;

namespace VPA.Usecases.Tests
{
	public class PatternManagerTests
	{
		[Fact]
		public void UpdateTree_Invokes_DesignPatternsChangedEvent()
		{
			var config = DefaultConfiguration.GetInstance();
			var patternManagerUsecase = config.GetService<IPatternManagerUsecase>();
			var projectNode = new ProjectNode();

			Assert.Raises<DesignPatternsChangedEventArgs>(e => patternManagerUsecase.DesignPatternsChangedEvent += e, e => patternManagerUsecase.DesignPatternsChangedEvent -= e, () => patternManagerUsecase.UpdateTree(projectNode));
		}
	}
}