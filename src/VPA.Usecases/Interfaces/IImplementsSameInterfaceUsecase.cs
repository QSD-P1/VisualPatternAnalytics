using VPA.Domain.Models;

namespace VPA.Usecases.Interfaces
{
	public interface IImplementsSameInterfaceUsecase
	{
		public bool Execute(IEnumerable<InterfaceNode> interfaceNodes, ClassNode classNode1, ClassNode classNode2, out InterfaceNode? matchedResult);
	}
}
