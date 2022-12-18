using VPA.Domain.Models;

namespace VPA.Usecases.Interfaces
{
	public interface IAnalyzeSingletonUsecase
	{
		public Task Analyze(IEnumerable<BaseNode> tree); 
	}
}
