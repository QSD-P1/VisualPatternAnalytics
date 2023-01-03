using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using VPA.Client.VisualStudio.Extension.VSIX.TreeViewItemEventHandlers;
using VPA.Domain.Models;

namespace VPA.Client.VisualStudio.Extension.VSIX.Adapters
{
	public class DetectorResultCollectionToTreeViewToTreeViewAdapter : IDetectorResultCollectionToTreeViewAdapter
	{
		public TreeViewItem Adapt(DetectionResultCollection detectionResultCollection)
		{
			if (detectionResultCollection is null)
			{
				throw new NullReferenceException("detectionResults is not set.");
			}

			// The design pattern that's detected
			var patternItem = new TreeViewItem()
			{
				Header = detectionResultCollection.DetectedPatternName,
				Name = detectionResultCollection.DetectedPatternName,
			};

			//We add all locations of the mainnodes of the detectedItems.
			//This way we can check if a certain location belongs to the pattern
			var tempLocationList = new List<object>();
			detectionResultCollection.Results.ForEach(x => tempLocationList.AddRange(x.DetectedItems.Select(a => a.MainNode.Location)));
			patternItem.Tag = tempLocationList;

			foreach (var detectionResult in detectionResultCollection.Results)
			{
				var resultItem = new TreeViewItem()
				{
					//Strip the whitespaces because its not allowed in a TreeViewItem
					Header = detectionResult.Name.Replace(" ", ""),
					Name = detectionResult.Name.Replace(" ", ""),
					Tag = detectionResult.DetectedItems.First().MainNode.Location
				};

				patternItem.Items.Add(ConvertDetectedItems(resultItem, detectionResult));
			}

			return patternItem;
		}

		private TreeViewItem ConvertDetectedItems(TreeViewItem resultItem, DetectionResult detectionResult)
		{
			foreach (DetectedItem detectedItem in detectionResult.DetectedItems)
			{
				var filepath = ((IEnumerable<Location>)detectedItem.MainNode.Location).First().SourceTree.FilePath;

				var mainNodeItem = new TreeViewItem()
				{
					Header = CreateHeaderTextblock(detectedItem.MainNode.Name, detectedItem.MainNode.ObjectTypeName, Path.GetFileName(filepath)),
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
						Header = CreateHeaderTextblock(leaf.Name, leaf.ObjectTypeName, $"Line {locationString}"),
						Name = leaf.Name,
						Tag = leaf.Location,
					};
					newItem.PreviewMouseDoubleClick += MouseDoubleClickEventHandler.OpenLocationInActiveFrame;
					mainNodeItem.Items.Add(newItem);
				}

				resultItem.Items.Add(mainNodeItem);
			}

			return resultItem;
		}

		private TextBlock CreateHeaderTextblock(string name, string type, string additionalInformation)
		{
			var result = new TextBlock();
			result.FontWeight = FontWeights.Bold;
			result.Text = name;

			var typeRun = new Run($" : {type}");
			typeRun.FontWeight = FontWeights.Regular;
			result.Inlines.Add(typeRun);

			var additionalInformationRun = new Run($" ({additionalInformation})");
			additionalInformationRun.FontStyle = FontStyles.Italic;
			additionalInformationRun.FontWeight = FontWeights.Light;
			result.Inlines.Add(additionalInformationRun);

			return result;
		}
	}
}