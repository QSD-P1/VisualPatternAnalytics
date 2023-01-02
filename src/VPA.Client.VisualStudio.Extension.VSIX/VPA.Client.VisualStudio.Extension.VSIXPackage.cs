global using Community.VisualStudio.Toolkit;
global using Microsoft.VisualStudio.Shell;
global using System;
global using Task = System.Threading.Tasks.Task;
using System.Runtime.InteropServices;
using System.Threading;
using VPA.Client.VisualStudio.Extension.VSIX.ToolWindows;

namespace VPA.Client.VisualStudio.Extension.VSIX
{
	[PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
	[InstalledProductRegistration(Vsix.Name, Vsix.Description, Vsix.Version)]
	[ProvideToolWindow(typeof(DesignPatternToolWindow.Pane), Style = VsDockStyle.Tabbed, Window = WindowGuids.SolutionExplorer)]
	[ProvideMenuResource("Menus.ctmenu", 1)]
	[Guid(PackageGuids.VPAVSIXString)]
	public sealed class VPAVSIXPackage : ToolkitPackage
	{
		protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
		{
			await this.RegisterCommandsAsync();

			VS.Events.SolutionEvents.OnAfterBackgroundSolutionLoadComplete += () => VS.Commands.ExecuteAsync("Build.RunCodeAnalysisonSolution");
			VS.Events.DocumentEvents.Saved += (input) => VS.Commands.ExecuteAsync("Build.RunCodeAnalysisonSolution");

			this.RegisterToolWindows();
		}
	}
}