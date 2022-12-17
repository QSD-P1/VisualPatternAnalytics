using System;
using System.Collections.Generic;
using System.Text;
using VPA.Domain.Parser;

namespace VPA.Domain.Parser
{
	public class MethodObject : Node
	{
		public AccessModifier AccessModifiers;
		public string? ReturnType { get; set; }
		public string[]? Parameters { get; set; }
		public object? Location { get; set; }
	}
}
