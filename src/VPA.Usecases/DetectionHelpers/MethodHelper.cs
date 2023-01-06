using VPA.Domain.Models;

namespace VPA.Usecases.DetectionHelpers
{
	public static class MethodHelper
	{
		public static bool HasSameClassReturnTypeWithKeywords(ClassNode classNode, KeywordCollection? keywordCollection, out MethodNode? matchedResult)
		{
			matchedResult = null;

			foreach (MethodNode methodNode in classNode.Children.OfType<MethodNode>())
			{
				if (methodNode.ReturnType.Equals(classNode.Name) && 
					(keywordCollection?.AccessModifier == null || keywordCollection.AccessModifier == methodNode.AccessModifier) && 
					(keywordCollection?.Modifiers == null || keywordCollection.Modifiers.All(x => methodNode.Modifiers?.Contains(x) ?? false)))
				{

					matchedResult = methodNode;
					return true;
				}
			}
			return false;
		}
	}
}