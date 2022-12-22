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

			// The design pattern thats detected
			var patternItem = new TreeViewItem()
			{
				Header = _adaptee.Name,
				Name = _adaptee.Name
			};

			foreach (DetectorResult detectorResult in _adaptee.Results)
			{
				foreach (DetectedItem detectedItem in detectorResult.Items)
				{
					var mainNodeItem = new TreeViewItem()
					{
						Header = detectedItem.MainNode.Name,
						Name = detectedItem.MainNode.Name
					};

					foreach (BaseLeaf leaf in detectedItem.Children)
					{
						mainNodeItem.Items.Add(new TreeViewItem()
						{
							Header = leaf.Name,
							Name = leaf.Name
						});
					}

					patternItem.Items.Add(mainNodeItem);
				}
			}

			_items.Add(patternItem);

			return _items;
		}
	}
}