using System;
using System.Collections.Generic;
using System.Text;
using VPA.Domain.Models;

namespace VPA.Usecases.Interfaces
{
	public interface ICheckForSameInterfaceImplementation
	{
		public bool Execute(IEnumerable<InterfaceNode> interfaceNodes, ClassNode classNode1, ClassNode classNode2, out InterfaceNode? matchedResult);
	}
}
