using System;
using System.Collections.Generic;
using System.Text;
using VPA.Domain.Enums;
using VPA.Domain.Models;

namespace VPA.Usecases.DetectionHelpers
{
	public static class AccessModifierHelper
	{
		public static bool AllTypeOfHasAccessModifier<T>(IEnumerable<BaseLeaf> nodes, AccessModifierEnum modifier, out List<T> leaves) where T : BaseLeaf
		{
			var nodesOfT = nodes.OfType<T>();
			leaves = nodesOfT.Where(n => n.AccessModifier == modifier).ToList();
			return leaves.Count() != nodesOfT.Count();
		}
	}
}