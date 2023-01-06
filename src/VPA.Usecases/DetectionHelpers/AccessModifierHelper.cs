using VPA.Domain.Enums;
using VPA.Domain.Models;

namespace VPA.Usecases.DetectionHelpers
{
	public static class AccessModifierHelper
	{
		public static bool AllOfTypeHasAccessModifier<T>(IEnumerable<BaseLeaf> nodes, AccessModifierEnum accessModifier, out List<T>? matchedLeaves) where T : BaseLeaf
		{
			// initialize the out param
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