using System.Windows.Controls;
using VPA.Domain.Models;

namespace VPA.Client.VisualStudio.Extension.VSIX.Adapters
{
	public interface IDetectorResultCollectionToTreeViewAdapter
	{
		/**
		 * Adapt the adaptee to something else
		 */
		public TreeViewItem Adapt(DetectorResultCollection resultCollection);
	}
}
