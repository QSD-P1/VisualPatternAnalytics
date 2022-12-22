using System.Collections.Generic;
using System.Windows.Controls;
using VPA.Common.Adapters.Interfaces.Presentation;
using VPA.Domain.Models;

namespace VPA.Client.VisualStudio.Extension.VSIX.Adapters.Presentation
{
	public class DetectorResultCollectionToTreeViewAdapter : IDetectorResultCollectionAdapter
	{
		private readonly List<TreeViewItem> _items;

		public DetectorResultCollectionToTreeViewAdapter()
		{
			_items = new();
		}

		public dynamic Adapt(DetectorResultCollection _adaptee)
		{
			if (_adaptee is null)
				throw new NullReferenceException("Adaptee is not set.");

			// The design pattern that's detected
			var patternItem = new TreeViewItem() { Header = _adaptee.Name };

			foreach (DetectorResult detectorResult in _adaptee.Results)
			{
				foreach (DetectedItem detectedItem in detectorResult.Items)
				{
					// TODO: Parse Type
					var mainNodeItem = new TreeViewItem() { Header = detectedItem.MainNode.GetType().Name };

					// These should be Fields, Methods, Constructor etc.
					foreach (BaseLeaf leaf in detectedItem.Children)
					{
						// TODO: Parse Type
						mainNodeItem.Items.Add(new TreeViewItem() { Header = leaf.Name ?? leaf.GetType().Name });
					}

					patternItem.Items.Add(mainNodeItem);
				}
			}

			_items.Add(patternItem);

			return _items;
		}
	}
}
