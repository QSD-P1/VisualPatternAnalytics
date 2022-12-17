using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace VPA.Common.Adapters.Parser
{
	public class ClassNode: Node
	{
		public string[] AccessModifiers = new string[6];
		public string? Abstraction { get; set; }
		public bool? Inherit { get; set; }
		public string[] Inherited { get; set; }
		public string? SourcePath { get; set; }
		public object? Location { get; set; }

		public List<Node> nodes;
		public ClassNode()
		{
			this.nodes = new List<Node>();
		}
	}
}
