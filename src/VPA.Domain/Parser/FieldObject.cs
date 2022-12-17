using System;
using System.Collections.Generic;
using System.Text;
using VPA.Domain.Parser;

namespace VPA.Common.Adapters.Parser
{
	public class FieldObject : Node
	{
		public AccessModifier AccessModifiers;
		public string? Type { get; set; }
		public object? Location { get; set; }
	}
}
