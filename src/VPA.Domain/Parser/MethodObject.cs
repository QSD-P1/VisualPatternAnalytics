using System;
using System.Collections.Generic;
using System.Text;

namespace VPA.Common.Adapters.Parser
{
	public class MethodObject : Node
	{
		public string[] AccessModifiers = new string[6];
		public string? ReturnType { get; set; }
		public string? Parameters { get; set; }
		public object? Location { get; set; }
	}
}
