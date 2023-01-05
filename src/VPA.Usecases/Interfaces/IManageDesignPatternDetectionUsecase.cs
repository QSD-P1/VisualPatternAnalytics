using VPA.Domain.Models;
using VPA.Usecases.Models;

namespace VPA.Usecases.Interfaces
{
	public interface IManageDesignPatternDetectionUsecase
	{
		public event EventHandler<DesignPatternsChangedEventArgs> DesignPatternsChangedEvent;
		public Task UpdateTree(ProjectNode node);
	}
}
