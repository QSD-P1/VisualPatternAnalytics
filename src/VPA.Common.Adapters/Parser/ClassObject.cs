using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace VPA.Common.Adapters.Parser
{
	public class ClassObject: Node
	{
		public string? Name { get; set; }
		public string[] AccessModifiers = new string[6];
		public string? Abstraction { get; set; }
		public bool? Inherit { get; set; }
		public string? SourcePath { get; set; }

		public List<Node> nodes;
		public ClassObject()
		{
			this.nodes = new List<Node>();
		}
	}
}
