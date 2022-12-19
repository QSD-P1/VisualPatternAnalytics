using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using VPA.Domain.Models;

namespace VPA.Common.Adapters.Interfaces.Presentation
{
	public interface IProjectTreeToTreeViewAdapter
	{
		public List<TreeViewItem> AdaptProjectTree(ProjectNode projectNode);
	}
}
