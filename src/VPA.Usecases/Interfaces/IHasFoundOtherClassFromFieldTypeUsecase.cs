using VPA.Domain.Models;

namespace VPA.Usecases.Interfaces
{
	public interface IHasFoundOtherClassFromFieldTypeUsecase
	{
		public bool Execute(IEnumerable<ClassNode> classNodes, FieldNode fieldNode, ClassNode classNode, out ClassNode? matchedResult);
	}
}
