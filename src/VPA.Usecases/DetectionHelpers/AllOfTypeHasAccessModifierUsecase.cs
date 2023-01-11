using VPA.Domain.Enums;
using VPA.Domain.Models;
using VPA.Usecases.Interfaces;

namespace VPA.Usecases.DetectionHelpers
{
	public class AllOfTypeHasAccessModifierUsecase : IAllOfTypeHasAccessModifierUsecase
	{
		public bool Execute<T>(IEnumerable<BaseLeaf> nodes, AccessModifierEnum accessModifier, out List<T>? matchedLeaves)
			where T : BaseLeaf
		{
			matchedLeaves = null;

			// Get only the nodes of the correct type
			IEnumerable<T> nodesOfT = nodes.OfType<T>();

			// Return false if some nodes doesnt contain the correct accessmodifier
			if (nodesOfT.Any(n => n.AccessModifier != accessModifier)) return false;

			// Set matchedLeaves and return true
			matchedLeaves = nodesOfT.ToList();
			return true;
		}
	}
}