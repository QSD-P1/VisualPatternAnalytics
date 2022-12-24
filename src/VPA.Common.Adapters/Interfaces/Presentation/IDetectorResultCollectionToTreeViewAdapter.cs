using System.Windows.Controls;
using VPA.Domain.Models;

namespace VPA.Common.Adapters.Interfaces.Presentation
{
	public interface IDetectorResultCollectionToTreeViewAdapter
	{
		/**
		 * Adapt the adaptee to something else
		 */
		public TreeViewItem Adapt(DetectorResultCollection resultCollection);
	}
}
