using System;
using System.Collections.Generic;
using System.Text;

namespace VPA.Domain.Parser
{
	public class ProjectNode: Node
	{
		public List<Node> nodes;

		public ProjectNode() 
		{
			this.nodes = new List<Node>();
		}
	}
}
