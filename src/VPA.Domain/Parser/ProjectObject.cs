using System;
using System.Collections.Generic;
using System.Text;

namespace VPA.Domain.Parser
{
	public class ProjectObject: Node
	{
		public List<Node> nodes;

		public ProjectObject() 
		{
			this.nodes = new List<Node>();
		}
	}
}
