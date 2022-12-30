using Microsoft.VisualStudio.Threading;
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
			DocumentView documentView = default;
			try
			{
				documentView = await obj.NewFrame.GetDocumentViewAsync();
			}
			catch (COMException) 
			{
				//The newframe isnt ready yet. Just return
				return;

			}

			// If the open window is not a document, we shouldn't do anything
			if (documentView == null)
			{
				return;
			}

			var currentSelectedItem = ClassTreeView.SelectedItem as TreeViewItem;
			var currentOpenDocumentFileName = obj.NewFrame.Caption;

			// PoC
			ActiveDocument.Text = obj.NewFrame.Caption;

			// If current open document is already selected, return
			if (currentSelectedItem != null && currentSelectedItem.Name == currentOpenDocumentFileName)
				return;

			List<TreeViewItem> foundItems = new();

			foreach (var rootItem in _treeItems)
			{
				foundItems.AddRange(FindItem(rootItem, currentOpenDocumentFileName));
			}

			CollapseAll(ClassTreeView.Items);

			// Do nothing if no item is found
			if (foundItems.Count == 0)
				return;

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

	private List<TreeViewItem> FindItem(TreeViewItem itemToCheck, string tagToFind)
	{
		var foundItems = new List<TreeViewItem>();

		if ((string)itemToCheck.Tag == tagToFind)
			foundItems.Add(itemToCheck);

		foreach (TreeViewItem item in itemToCheck.Items)
		{
			if ((string)item.Tag == tagToFind)
			{
				foundItems.Add(item);
			}

			if (item.HasItems)
			{
				foreach (TreeViewItem childItem in item.Items)
				{
					foundItems.AddRange(FindItem(childItem, tagToFind));
				}
			}
			else
			{
				continue;
			}
		}

		// nothing found
		return foundItems;
	}
}
}