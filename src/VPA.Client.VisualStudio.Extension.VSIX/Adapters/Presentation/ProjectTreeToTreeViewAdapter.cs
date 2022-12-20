using System.Collections.Generic;
using System.Windows.Controls;
using VPA.Common.Adapters.Interfaces.Presentation;
using VPA.Domain.Models;

namespace VPA.Client.VisualStudio.Extension.VSIX.Adapters.Presentation
{
	public class ProjectTreeToTreeViewAdapter : IProjectTreeAdapter
	{
		private ProjectNode _adaptee;
		private readonly List<TreeViewItem> _items;

		public ProjectTreeToTreeViewAdapter()
		{
			_items = new();
		}

		public void SetAdaptee(ProjectNode projectNode)
		{
			_adaptee = projectNode;
		}

		public dynamic Adapt(dynamic source)
		{
			if (_adaptee is null)
			{
				throw new NullReferenceException("Adaptee is not set.");
			}

			// TODO: Adapt the adaptee instead of the mock data

			var singleTon = new TreeViewItem() { Tag = "singleton1", Header = "Singleton pattern",  };
			var testItem = new TreeViewItem() { Tag = "programcs", Header = "Program.cs" };
			testItem.Items.Add(new TreeViewItem() { Tag = "Test2", Header = "Test 2" });
			testItem.Items.Add(new TreeViewItem() { Tag = "Test3", Header = "Test 3" });
			singleTon.Items.Add(testItem);
			_items.Add(singleTon);

			var factoryPattern = new TreeViewItem() { Tag = "factorypattern1", Header = "Factory pattern" };
			var testItem2 = new TreeViewItem() { Tag = "DesignPatternToolWindow.cs", Header = "DesignPatternToolWindow.cs" };
			testItem2.Items.Add(new TreeViewItem() { Tag = "Test5", Header = "Test 5" });
			testItem2.Items.Add(new TreeViewItem() { Tag = "Test6", Header = "Test 6" });
			factoryPattern.Items.Add(testItem2);
			_items.Add(factoryPattern);

			var testing = new TreeViewItem() { Tag = "testing", Header = "Testing multiple hits" };
			var testItem3 = new TreeViewItem() { Tag = "DesignPatternToolWindow.cs", Header = "DesignPatternToolWindow.cs" };
			testItem3.Items.Add(new TreeViewItem() { Tag = "Test7", Header = "Test 7" });
			testing.Items.Add(testItem3);
			_items.Add(testing);

			return _items;
		}
	}
}
