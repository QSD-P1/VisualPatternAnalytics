using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

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

			#region prepare tree items
			var singleTon = new TreeViewItem() { Name = "singleton1", Header = "Singleton pattern" };
			var testItem = new TreeViewItem() { Name = "programcs", Header = "Program.cs" };
			testItem.Items.Add(new TreeViewItem() { Name = "Test2", Header = "Test  2" });
			testItem.Items.Add(new TreeViewItem() { Name = "Test3", Header = "Test 3" });
			singleTon.Items.Add(testItem);

			var factoryPattern = new TreeViewItem() { Name = "factorypattern1", Header = "Factory pattern" };
			var testItem2 = new TreeViewItem() { Name = "testclasscs", Header = "TestClass.cs" };
			testItem2.Items.Add(new TreeViewItem() { Name = "Test5", Header = "Test 5" });
			testItem2.Items.Add(new TreeViewItem() { Name = "Test6", Header = "Test 6" });
			factoryPattern.Items.Add(testItem2);
			_treeItems.Add(singleTon);
			_treeItems.Add(factoryPattern);
			#endregion

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