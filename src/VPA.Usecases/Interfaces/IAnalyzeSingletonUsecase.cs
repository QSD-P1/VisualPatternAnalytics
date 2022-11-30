using VPA.Domain.Models;

namespace VPA.Usecases.Interfaces
{
	public interface IAnalyzeSingletonUsecase
	{
		public Task Analyze(GenericTree tree); 
	}
}
