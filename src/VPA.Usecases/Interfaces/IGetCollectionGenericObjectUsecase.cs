using VPA.Domain.Models;

namespace VPA.Usecases.Interfaces
{
	public interface IGetCollectionGenericObjectUsecase
	{
		public CollectionGenericObject Execute(string typeString);
	}
}
