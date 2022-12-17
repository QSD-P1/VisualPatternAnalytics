using System;
using System.Collections.Generic;
using System.Text;

namespace VPA.Common.Adapters.Parser
{
	public class ConstructorObject: Node
	{
		public string? AccessModifier { get; set; }
		public string[]? Parameters;
	}
}
