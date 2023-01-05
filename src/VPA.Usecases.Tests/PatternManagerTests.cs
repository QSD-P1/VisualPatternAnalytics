using VPA.Configuration;
using VPA.Domain.Models;
using VPA.Usecases.DetectionHelpers;
using VPA.Usecases.Detectors;
using VPA.Usecases.Interfaces;
using VPA.Usecases.Manager;
using VPA.Usecases.Models;
using VPA.Usecases.Usecases;

namespace VPA.Usecases.Tests
{
	public class PatternManagerTests
	{
		[Fact]
		public void UpdateTree_Invokes_DesignPatternsChangedEvent()
		{
			var patternManagerUsecase = new PatternManagerUsecase(
				new DetectSingletonUsecase(),
				new DetectCompositeUsecase(
					new GetClassesPerParentClassUsecase(),
					new GetClassesPerInterfaceUsecase(),
					new GetClassesWithParentListTypeUsecase(
						new GetCollectionGenericObjectUsecase()),
					new GetCollectionGenericObjectUsecase()));
				var projectNode = new ProjectNode();

			Assert.Raises<DesignPatternsChangedEventArgs>(e => patternManagerUsecase.DesignPatternsChangedEvent += e, e => patternManagerUsecase.DesignPatternsChangedEvent -= e, () => patternManagerUsecase.UpdateTree(projectNode));
		}
	}
}