using System;
using System.Collections.Generic;
using System.Text;
using VPA.Domain.Parser;

namespace VPA.Domain.Parser
{
	public class ConstructorNode: Node
	{
		public AccessModifier AccessModifiers;
		public string[]? Parameters { get; set; }
	}
}
