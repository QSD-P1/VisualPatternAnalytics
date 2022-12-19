using System;
using System.Collections.Generic;
using System.Text;
using VPA.Domain.Parser;

namespace VPA.Domain.Parser
{
	public class FieldNode: Node
	{
		public AccessModifier AccessModifiers;
		public string? Type { get; set; }
		public object? Location { get; set; }
	}
}
