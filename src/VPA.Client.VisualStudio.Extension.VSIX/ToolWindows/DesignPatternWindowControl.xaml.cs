using Microsoft.VisualStudio.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using VPA.Client.VisualStudio.Extension.VSIX.Adapters;
using VPA.Configuration;
using VPA.Usecases.Interfaces;
using VPA.Usecases.Models;

namespace VPA.Client.VisualStudio.Extension.VSIX.ToolWindows
{
	public partial class DesignPatternWindowControl : UserControl
	{
		private List<TreeViewItem> _treeItems = new();
		private readonly IPatternManagerUsecase _patternManager;
		private readonly IDetectorResultCollectionToTreeViewAdapter _adapter;
		public DesignPatternWindowControl()
		{
			InitializeComponent();

			var configuration = DefaultConfiguration.GetInstance();
			_patternManager = configuration.GetService<IPatternManagerUsecase>();
			_adapter = new DetectorResultCollectionToTreeViewToTreeViewAdapter();

			Init();
		}

		private void Init()
		{
			VS.Events.WindowEvents.ActiveFrameChanged += WindowEvents_ActiveFrameChanged;
			ClassTreeView.SelectedItemChanged += ClassTreeView_SelectedItemChanged;

			_patternManager.DesignPatternsChangedEvent += PatternManagerDesignPatternsChangedEventHandler;

			ClassTreeView.ItemsSource = _treeItems;
		}

		private void PatternManagerDesignPatternsChangedEventHandler(object patternManager, DesignPatternsChangedEventArgs eventArgs)
		{
			ThreadHelper.JoinableTaskFactory.Run(async delegate
			{
				await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
				await HandleEventAsync(eventArgs);
			});
		}

		private Task HandleEventAsync(DesignPatternsChangedEventArgs eventArgs)
		{
			var tempItems = new List<TreeViewItem>();
			foreach (var resultCollection in eventArgs.Result.Where(y => y.Results.Any()))
			{
				tempItems.Add(_adapter.Adapt(resultCollection));
			}

			_treeItems = tempItems;
			ClassTreeView.ItemsSource = _treeItems;

			return Task.CompletedTask;
		}

		private async void WindowEvents_ActiveFrameChanged(ActiveFrameChangeEventArgs obj)
		{
			DocumentView? documentView = null;
			try
			{
				documentView = await obj.NewFrame.GetDocumentViewAsync();
			}
			catch (COMException)
			{
				//The newframe isnt ready yet. Just return
				return;

			}

			//If the open window is not a document, we shouldn't do anything
			//Also if the selectedItem is'nt a treeviewItem, we should do nothing
			if (documentView == null)
			{
				return;
			}

			var currentOpenDocumentFileName = obj.NewFrame.Caption;
			ActiveDocument.Text = currentOpenDocumentFileName;

			//If current open document is already selected, return
			if (ClassTreeView.SelectedItem is not TreeViewItem currentSelectedItem || currentSelectedItem.Name == currentOpenDocumentFileName)
			{
				return;
			}

			List<TreeViewItem> foundItems = new();
			foreach (var rootItem in _treeItems)
			{
				foundItems.AddRange(FindItemsMatchingTheCurrentLocation(rootItem, currentOpenDocumentFileName));
			}

			//We collapse everything to make sure nothing is expanded
			CollapseAll(ClassTreeView.Items);

			//We expand everything that matched the location
			foundItems.ForEach(x => x.IsExpanded = true);

			ClassTreeView.UpdateLayout();
		}

		private void ClassTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			var selectedItem = e.NewValue as TreeViewItem;

			//HandleExpansion(selectedItem);
			ActiveNode.Text = selectedItem.Header as string;
		}

		//private void HandleExpansion(TreeViewItem itemToExpand)
		//{
		//	foreach (var treeItem in _treeItems)
		//	{
		//		treeItem.IsExpanded = false;
		//	}

		//	var parent = itemToExpand.Parent as TreeViewItem;
		//	itemToExpand.IsExpanded = true;

		//	while (parent != null)
		//	{
		//		parent.IsExpanded = true;
		//		parent = parent.Parent as TreeViewItem;
		//	}
		//}

		private void CollapseAll(ItemCollection treeItems)
		{
			foreach (TreeViewItem item in treeItems)
			{
				item.IsExpanded = false;

				if (item.HasItems)
				{
					CollapseAll(item.Items);
				}
			}
		}

		private List<TreeViewItem> FindItemsMatchingTheCurrentLocation(TreeViewItem itemToCheck, string locationToFind)
		{
			var foundItems = new List<TreeViewItem>();

			//A tag should be a IEnumerable since we can have multiple tags
			//We use the tag to store the location of a item
			if (itemToCheck.Tag is IEnumerable<object> nestedCollection && CheckIfChildItemsMatchTheLocationToFind(nestedCollection, locationToFind))
			{
				foundItems.Add(itemToCheck);
			}

			//If the item has child TreeViewItems, we should also check them
			if (itemToCheck.HasItems)
			{
				foreach (TreeViewItem childItem in itemToCheck.Items)
				{
					foundItems.AddRange(FindItemsMatchingTheCurrentLocation(childItem, locationToFind));
				}
			}

			return foundItems;
		}

		/// <summary>
		/// Checks if the given collection contains the location you are looking for.
		/// </summary>
		/// <param name="tagCollection"></param>
		/// <param name="locationToFind"></param>
		/// <returns>Bool indicating if the collection contains the location. True == yes, False == no</returns>
		private bool CheckIfChildItemsMatchTheLocationToFind(IEnumerable<object> tagCollection, string locationToFind)
		{
			foreach (var tag in tagCollection)
			{
				//If the item has more childs, we should loop through the childs
				if (tag is IEnumerable<object> nestedCollection)
				{
					return CheckIfChildItemsMatchTheLocationToFind(nestedCollection, locationToFind);
				}
				//If we found the leaves of the tree, we check if it matches the location we are looking for
				else if (tag.ToString().Contains(locationToFind))
				{
					return true;
				}
			}

			//We could'nt find the location, so we return false
			return false;
		}
	}
}