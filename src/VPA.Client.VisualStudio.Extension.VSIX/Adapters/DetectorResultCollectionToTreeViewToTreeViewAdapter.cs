using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using VPA.Client.VisualStudio.Extension.VSIX.TreeViewItemEventHandlers;
using VPA.Domain.Models;

namespace VPA.Client.VisualStudio.Extension.VSIX.Adapters
{
	public class DetectorResultCollectionToTreeViewToTreeViewAdapter : IDetectorResultCollectionToTreeViewAdapter
	{
		public TreeViewItem Adapt(DetectorResultCollection detectionResults)
		{
			if (detectionResults is null)
			{
				throw new NullReferenceException("detectionResults is not set.");
			}
			// The design pattern that's detected
			var patternItem = new TreeViewItem()
			{
				Header = detectionResults.Name,
				Name = detectionResults.Name,
				Tag = detectionResults.Results.Select(x => x.MainNode.Location)
			};

			foreach (DetectedItem detectedItem in detectionResults.Results)
			{
				var filepath = ((IEnumerable<Location>)detectedItem.MainNode.Location).First().SourceTree.FilePath;
				Path.GetFileName(filepath);
				var headerText = $"{detectedItem.MainNode.Name} : {detectedItem.MainNode.ObjectTypeName} ({Path.GetFileName(filepath)})";

				var mainNodeItem = new TreeViewItem()
				{
					Header = headerText,
					Name = detectedItem.MainNode.Name,
					Tag = detectedItem.MainNode.Location,
				};

				mainNodeItem.PreviewMouseDoubleClick += MouseDoubleClickEventHandler.OpenLocationInActiveFrame;

				foreach (BaseLeaf leaf in detectedItem.Children)
				{
					//Get the linenumber from the location + 1
					//We need to add 1 since it starts counting from 0, but the visual interface doesn't
					var locationString = ((IEnumerable<Location>)leaf.Location).First().GetLineSpan().StartLinePosition.Line + 1;

					var newItem = new TreeViewItem()
					{
						Header = $"{leaf.Name} : {leaf.ObjectTypeName} (Line {locationString})",
						Name = leaf.Name,
						Tag = leaf.Location,
					};
					newItem.PreviewMouseDoubleClick += MouseDoubleClickEventHandler.OpenLocationInActiveFrame;
					mainNodeItem.Items.Add(newItem);
				}

				patternItem.Items.Add(mainNodeItem);
			}

			return patternItem;
		}
	}
}