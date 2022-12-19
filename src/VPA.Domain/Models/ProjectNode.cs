using System;
using System.Collections.Generic;
using System.Text;

namespace VPA.Domain.Models
{
	public class ProjectNode
	{
		public IEnumerable<ClassNode>? ClassNodes { get; set; }
	}
}
