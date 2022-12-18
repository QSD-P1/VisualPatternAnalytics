using VPA.Domain.Models;
using VPA.Usecases.Interfaces;

namespace VPA.Usecases.Usecases
{
	public class AnalyzeSingletonUsecase : IAnalyzeSingletonUsecase
	{
		private readonly IAnalyzeFactoryUsecase analyzeSingletonUsecase;

		public AnalyzeSingletonUsecase(IAnalyzeFactoryUsecase analyzeSingletonUsecase) 
		{
			this.analyzeSingletonUsecase = analyzeSingletonUsecase;
		}
		public Task Analyze(IEnumerable<BaseNode> tree)
		{
			analyzeSingletonUsecase.Analyze(tree);
			throw new NotImplementedException();
		}
	}
}