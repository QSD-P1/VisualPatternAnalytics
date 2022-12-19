using VPA.Domain.Models;

namespace VPA.Usecases.Interfaces
{
	public interface IAnalyzeFactoryUsecase
	{
		public Task Analyze(ProjectNode tree);
	}
}
