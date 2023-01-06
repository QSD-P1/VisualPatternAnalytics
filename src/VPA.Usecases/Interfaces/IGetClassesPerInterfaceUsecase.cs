using VPA.Domain.Models;

namespace VPA.Usecases.Interfaces
{
	public interface IGetClassesPerInterfaceUsecase
	{
		public Dictionary<string, List<ClassNode>> Execute(ProjectNode projectNode);
	}
}
