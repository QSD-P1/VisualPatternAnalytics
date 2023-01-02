using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TextManager.Interop;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace VPA.Client.VisualStudio.Extension.VSIX.TreeViewItemEventHandlers
{
	internal static class MouseDoubleClickEventHandler
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "VSTHRD100:Avoid async void methods", Justification = "This is an eventhandler, they are the only valid usage of async void")]
		public static async void OpenLocationInActiveFrame(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (e.Source is not TreeViewItem treeViewItem)
			{
				return;
			}

			Location location;
			if (treeViewItem.Tag is IEnumerable<object> locations)
			{
				location = (Location)locations.First();
			}
			else
			{
				location = (Location)treeViewItem.Tag;
			}

			//Open the document in VS
			var view = await VS.Documents.OpenAsync(location.SourceTree.FilePath);

			var vsTextView = await view.TextView.ToIVsTextViewAsync();
			var line = location.GetLineSpan();

			//Select the appropiate line
			vsTextView.SetSelection(line.StartLinePosition.Line, 0, line.EndLinePosition.Line, line.EndLinePosition.Character);

			//Set handled so possible parent onclicks wont be fired
			e.Handled = true;
		}
	}
}
