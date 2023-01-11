using VPA.Domain.Enums;
using VPA.Domain.Models;

namespace VPA.Usecases.Interfaces
{
	public interface IAllOfTypeHasAccessModifierUsecase
	{
		bool Execute<T>(IEnumerable<BaseLeaf> nodes, AccessModifierEnum accessModifier, out List<T>? matchedLeaves) where T : BaseLeaf;
	}
}