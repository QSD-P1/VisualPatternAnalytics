using System;
using System.Collections.Generic;
using System.Text;

namespace VPA.Common.Adapters.Parser
{
	public class ProjectObject
	{
		public string? Name { get; set; }
		public List<Node> nodes;

		public ProjectObject() 
		{
			this.nodes = new List<Node>();
		}
	}
}
