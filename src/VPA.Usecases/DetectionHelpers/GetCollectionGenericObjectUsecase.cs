using VPA.Domain.Models;
using VPA.Usecases.Interfaces;

namespace VPA.Usecases.DetectionHelpers
{
	public class GetCollectionGenericObjectUsecase : IGetCollectionGenericObjectUsecase
	{
		public CollectionGenericObject Execute(string typeString)
		{
			if (typeString == null || typeString == "")
			{
				return null;
			}

			var genericTypeArray = typeString.Split('<');
			var collectionType = genericTypeArray.FirstOrDefault();

			if (collectionType == null)
			{
				return null;
			}

			var genericType = genericTypeArray.Length > 1 ? genericTypeArray[1] : null;
			genericType = genericType?.Split('>').FirstOrDefault();

			return new CollectionGenericObject { CollectionType = collectionType, GenericType = genericType };
		}
	}
}
