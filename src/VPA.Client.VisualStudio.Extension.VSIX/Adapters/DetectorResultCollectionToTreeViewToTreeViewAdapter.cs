﻿using System.Linq;
using System.Windows.Controls;
using VPA.Domain.Models;

namespace VPA.Client.VisualStudio.Extension.VSIX.Adapters
{
	public class DetectorResultCollectionToTreeViewToTreeViewAdapter : IDetectorResultCollectionToTreeViewAdapter
	{
		public TreeViewItem Adapt(DetectorResultCollection detectionResults)
		{
			if (detectionResults is null)
				throw new NullReferenceException("Adaptee is not set.");

			// The design pattern that's detected
			var patternItem = new TreeViewItem()
			{
				Header = detectionResults.Name,
				Name = detectionResults.Name,
				Tag = detectionResults.Results.Select(x => x.MainNode.Location)
			};

			foreach (DetectedItem detectedItem in detectionResults.Results)
			{
				var mainNodeItem = new TreeViewItem()
				{
					Header = detectedItem.MainNode.Name,
					Name = detectedItem.MainNode.Name,
					Tag = detectedItem.MainNode.Location,
				};

				foreach (BaseLeaf leaf in detectedItem.Children)
				{
					mainNodeItem.Items.Add(new TreeViewItem()
					{
						Header = leaf.Name,
						Name = leaf.Name,
						Tag = leaf.Location,
					});
				}

				patternItem.Items.Add(mainNodeItem);
			}

			return patternItem;
		}
	}
}