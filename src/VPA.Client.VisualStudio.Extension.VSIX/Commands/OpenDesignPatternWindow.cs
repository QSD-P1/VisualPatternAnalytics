using VPA.Client.VisualStudio.Extension.VSIX.ToolWindows;

namespace VPA.Client.VisualStudio.Extension.VSIX.Commands
{
	[Command(PackageIds.OpenDesignPatternWindow)]
	internal sealed class OpenDesignPatternWindow : BaseCommand<OpenDesignPatternWindow>
	{
		protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
		{
			await DesignPatternToolWindow.ShowAsync();
		}
	}
}
