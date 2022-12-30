using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.VisualStudio.Imaging;

namespace VPA.Client.VisualStudio.Extension.VSIX.ToolWindows
{
	public class DesignPatternToolWindow : BaseToolWindow<DesignPatternToolWindow>
	{
		public override string GetTitle(int toolWindowId) => "Design Pattern Window";

		public override Type PaneType => typeof(Pane);

		public override Task<FrameworkElement> CreateAsync(int toolWindowId, CancellationToken cancellationToken)
		{
			return Task.FromResult<FrameworkElement>(new DesignPatternWindowControl());
		}

		[Guid("6d2ba245-609f-450a-80ca-12556f6f9280")]
		internal class Pane : ToolWindowPane
		{
			public Pane()
			{
				BitmapImageMoniker = KnownMonikers.ToolWindow;
			}
		}
	}
}