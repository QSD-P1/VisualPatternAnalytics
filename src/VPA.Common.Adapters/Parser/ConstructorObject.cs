using System;
using System.Collections.Generic;
using System.Text;

namespace VPA.Common.Adapters.Parser
{
	public class ConstructorObject: Node
	{
		public string? Name { get; set; }
		public string? AccessModifiers;
		public string[]? Parameters;
		public object? Location { get; set; }
	}
}
