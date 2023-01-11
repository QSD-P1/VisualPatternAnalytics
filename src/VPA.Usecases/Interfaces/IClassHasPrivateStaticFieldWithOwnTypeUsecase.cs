using VPA.Domain.Models;

namespace VPA.Usecases.Interfaces
{
	public interface IClassHasPrivateStaticFieldWithOwnTypeUsecase
	{
		bool Execute(ClassNode node, out FieldNode leaf);
	}
}