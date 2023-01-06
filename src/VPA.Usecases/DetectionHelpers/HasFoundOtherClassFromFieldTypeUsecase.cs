using VPA.Domain.Models;
using VPA.Usecases.Interfaces;

namespace VPA.Usecases.DetectionHelpers
{
	public class HasFoundOtherClassFromFieldTypeUsecase: IHasFoundOtherClassFromFieldTypeUsecase
	{
		public bool Execute(IEnumerable<ClassNode> classNodes, FieldNode fieldNode, ClassNode classNode, out ClassNode? matchedResult)
		{
			// initialize the out param
			matchedResult = null;

			// Return false because field type is same class
			if (fieldNode.Type == classNode.Name) return false;

			// Search for the classnode
			matchedResult = classNodes.FirstOrDefault(x => x.Name == fieldNode.Type?.Replace("?", ""));

			return matchedResult != null;
		}
	}
}
