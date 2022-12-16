using System;
using System.Collections.Generic;
using System.Text;

namespace VPA.Common.Adapters.Parser
{
	public class ConstructorObject: Node
	{
		public string? Name { get; set; }
		public string[] AccessModifiers = new string[2];
		public string[]? Parameters;
		public object? Location { get; set; }
	}
}
