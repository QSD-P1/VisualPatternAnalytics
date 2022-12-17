using System;
using System.Collections.Generic;
using System.Text;
using VPA.Domain.Parser;

namespace VPA.Common.Adapters.Parser
{
	public class ConstructorObject: Node
	{
		public AccessModifier AccessModifiers;
		public string[]? Parameters;
	}
}
