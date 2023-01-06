using VPA.Domain.Enums;
using VPA.Domain.Models;
using VPA.Usecases.Interfaces;

namespace VPA.Usecases.DetectionHelpers
{
	public class GetClassesWithParentListTypeUsecase : IGetClassesWithParentListTypeUsecase
	{
		private readonly IGetCollectionGenericObjectUsecase _getCollectionGenericObject;

		public GetClassesWithParentListTypeUsecase(IGetCollectionGenericObjectUsecase getCollectionGenericObject)
		{
			_getCollectionGenericObject = getCollectionGenericObject;
		}

		public Dictionary<string, List<ClassNode>> Execute(Dictionary<string, List<ClassNode>> classesPerParent)
		{
			// Check if class has collection field of interface type
			var classesWithParentListType = new Dictionary<string, List<ClassNode>>();

			foreach (KeyValuePair<string, List<ClassNode>> entry in classesPerParent)
			{
				var currentParent = entry.Key;

				foreach (ClassNode classNode in entry.Value)
				{
					if (classNode.Children == null) continue;

					var fields = classNode.Children.OfType<FieldNode>().ToList();

					if (!fields.Any()) continue;

					foreach (FieldNode field in fields)
					{
						var collectionGenericObject = _getCollectionGenericObject.Execute(field.Type);

						if (!Enum.IsDefined(typeof(CollectionTypesEnum), collectionGenericObject.CollectionType) ||
						    collectionGenericObject.GenericType != currentParent) continue;

						// Found it!
						if (!classesWithParentListType.ContainsKey(currentParent))
							classesWithParentListType[currentParent] = new List<ClassNode>();

						classesWithParentListType[currentParent].Add(classNode);
					}
				}
			}

			return classesWithParentListType;
		}
	}
}