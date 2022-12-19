using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using VPA.Client.VisualStudio.Extension.VSIX.Adapters.Presentation;

namespace VPA.Client.VisualStudio.Extension.VSIX
{
	public partial class DesignPatternWindowControl : UserControl
	{
		public DesignPatternWindowControl()
		{
			InitializeComponent();
			Init();
		}

		private List<TreeViewItem> _treeItems = new List<TreeViewItem>();

		private void Init()
		{
			VS.Events.WindowEvents.ActiveFrameChanged += WindowEvents_ActiveFrameChanged;
			ClassTreeView.SelectedItemChanged += ClassTreeView_SelectedItemChanged;

			// TODO: Remove mock data
			var adapter = new ProjectTreeToTreeViewAdapter();
			adapter.SetAdaptee(new Domain.Models.ProjectNode());
			_treeItems = adapter.Adapt(new List<TreeViewItem>());

			ClassTreeView.ItemsSource = _treeItems;

		}

		private void WindowEvents_ActiveFrameChanged(ActiveFrameChangeEventArgs obj)
		{
			var documentView = obj.NewFrame.GetDocumentViewAsync().GetAwaiter().GetResult();

			// If the open window is not a document, we shouldn't do anything
			if (documentView == null)
				return;

			var currentTreeView = ClassTreeView.SelectedItem as TreeViewItem;
			var parsedName = obj.NewFrame.Caption.Replace(".", "").ToLowerInvariant();
			ActiveDocument.Text = obj.NewFrame.Caption;

			if (currentTreeView == null || currentTreeView.Name != parsedName)
			{
				TreeViewItem foundItem = null;

				foreach (var rootItem in _treeItems)
				{
					foundItem = FindItem(rootItem, parsedName);
					if (foundItem != null)
						break;
				}


				if (foundItem != null)
				{
					HandleExpansion(foundItem);
				}
			}

			ClassTreeView.UpdateLayout();
		}

		private void HandleExpansion(TreeViewItem itemToExpand)
		{
			foreach (var treeItem in _treeItems)
			{
				treeItem.IsExpanded = false;
			}

			var parent = itemToExpand.Parent as TreeViewItem;
			itemToExpand.IsExpanded = true;

			while (parent != null)
			{
				parent.IsExpanded = true;
				parent = parent.Parent as TreeViewItem;
			}
		}

		private TreeViewItem FindItem(TreeViewItem rootItem, string name)
		{
			var isRoot = rootItem.Name == name;
			if (isRoot)
				return rootItem;

			foreach (TreeViewItem item in rootItem.Items)
			{
				if (item.Name == name)
				{
					return item;
				}

				if (item.HasItems)
				{
					foreach (TreeViewItem childItem in item.Items)
					{
						return FindItem(childItem, name);
					}
				}
				else
				{
					continue;
				}
			}

			// nothing found
			return null;
		}

		private void ClassTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			var newTreeItem = e.NewValue as TreeViewItem;
			ActiveNode.Text = newTreeItem.Header as string;
		}
	}
}