using System;
using System.Collections.Generic;
using System.Text;
using VPA.Domain.Models;

namespace VPA.Usecases.DetectionHelpers
{
	public class InterfaceHelper
	{
		public static bool HasSameInterface(ClassNode classNode1, ClassNode classNode2, out InterfaceNode? matchedResult)
		{
			// initialize the out param
			matchedResult = null;

			// Searches for the interface
			string? matchedInterfaceName = classNode1.Interfaces.FirstOrDefault(x => classNode2.Interfaces.Contains(x));

			// If no interface found; return false
			if (matchedInterfaceName == null) return false;

			// Set matchedResult and return true
			matchedResult = new InterfaceNode()
			{
				Name = matchedInterfaceName
			};
			return true;
		}
	}
}
