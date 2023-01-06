using VPA.Domain.Models;

namespace VPA.Usecases.Interfaces
{
	public interface IGetClassFromFieldType
	{
		public bool Execute(IEnumerable<ClassNode> classNodes, FieldNode fieldNode, ClassNode classNode, out ClassNode? matchedResult);
	}
}
