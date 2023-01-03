using VPA.Domain.Enums;
using VPA.Domain.Models;

namespace VPA.Usecases.DetectionHelpers
{
	public static class FieldHelper
	{
		public static bool ClassHasPrivateStaticFieldWithOwnType(ClassNode node, out FieldNode leaf)
		{
			leaf = node.Children.OfType<FieldNode>()
			.Where(n => 
				   n.Modifiers.Contains(ModifiersEnum.Static) 
				&& n.AccessModifier == AccessModifierEnum.Private 
				&& n.Type?.Replace("?", "") == node.Name)
			.FirstOrDefault();

			return leaf != null;
		}

		public static CollectionGenericObject GetCollectionGenericObject(string typeString)
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