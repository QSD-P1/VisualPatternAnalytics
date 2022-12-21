using VPA.Domain.Models;
using VPA.Usecases.Models;
using static VPA.Usecases.Manager.PatternManagerUsecase;

namespace VPA.Usecases.Interfaces
{
	public interface IPatternManagerUsecase
	{
		public event EventHandler<DesignPatternsChangedEventArgs> DesignPatternsChangedEvent;
		public Task UpdateTree(ProjectNode node);
	}
}
