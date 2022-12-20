using System;
using System.Collections.Generic;
using System.Text;
using VPA.Domain.Enums;
using VPA.Domain.Models;

namespace VPA.Usecases.DetectionHelpers
{
	public static class AccessModifierHelper
	{
		public static bool AllTypeOfHasAccessModifier<T>(IEnumerable<BaseLeaf> nodes, AccessModifierEnum modifier) where T : BaseLeaf
		{
			foreach (var node in nodes.OfType<T>())
			{
				if (node.AccessModifier != modifier)
					return false;
			}
			return true;
		}
	}
}
