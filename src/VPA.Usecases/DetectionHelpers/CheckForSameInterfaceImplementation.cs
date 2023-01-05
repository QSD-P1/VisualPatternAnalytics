using System;
using System.Collections.Generic;
using System.Text;
using VPA.Domain.Models;
using VPA.Usecases.Interfaces;

namespace VPA.Usecases.DetectionHelpers
{
	public class CheckForSameInterfaceImplementation : ICheckForSameInterfaceImplementation
	{
		public bool Execute(IEnumerable<InterfaceNode> interfaceNodes, ClassNode classNode1, ClassNode classNode2, out InterfaceNode? matchedResult)
		{
			// initialize the out param
			matchedResult = null;

			// If one of them has no interfaces implemented; return false
			if (classNode2.Interfaces == null) return false;

			// Searches for the interface
			string? matchedInterfaceName = classNode1.Interfaces.FirstOrDefault(x => classNode2.Interfaces.Contains(x));

			// If no identical interface found; return false
			if (matchedInterfaceName == null) return false;

			// Set matchedResult and return true
			matchedResult = interfaceNodes.FirstOrDefault(x => x.Name == matchedInterfaceName);

			return true;
		}
	}
}
