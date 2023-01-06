using VPA.Domain.Models;

namespace VPA.Usecases.Interfaces
{
	public interface IDetectUsecase
	{
		public string PatternName { get; }

		public Task<DetectionResultCollection> Detect(ProjectNode project);
	}
}
