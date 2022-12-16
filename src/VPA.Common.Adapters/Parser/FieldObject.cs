using System;
using System.Collections.Generic;
using System.Text;

namespace VPA.Common.Adapters.Parser
{
	public class FieldObject : Node
	{
		public string? Name;
		public string[] AccessModifiers = new string[2];
		public string? Type { get; set; }
		public object? Location { get; set; }
	}
}
