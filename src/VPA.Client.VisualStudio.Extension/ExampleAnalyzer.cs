using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using VPA.Common.Adapters.Adapters;
using VPA.Common.Adapters.Interfaces;
using VPA.Configuration;
using VPA.Domain.Models;
using VPA.Usecases.Interfaces;

namespace VPA.Client.VisualStudio.Extension
{
	[DiagnosticAnalyzer(LanguageNames.CSharp)]
	public class ExampleAnalyzer : DiagnosticAnalyzer
	{
		public const string DiagnosticId = "VPAClientVisualStudioExtension";

		// You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
		// See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Localizing%20Analyzers.md for more on localization
		private static readonly string Title = "Title";
		private static readonly string MessageFormat = "Format";
		private static readonly string Description = "Description";
		private const string Category = "Naming";
		private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

		public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

		private readonly IRoslynAdapter roslynAdapter;
		private readonly IPatternManagerUsecase patternManager;

		public ExampleAnalyzer()
		{
			//Sadly analyzers dont contain MEF so we cant use Dependency injection.
			//We wrote our own singleton that manages the implementations.
			var config = DefaultConfiguration.GetInstance();
			roslynAdapter = config.GetService<IRoslynAdapter>();
			patternManager = config.GetService<IPatternManagerUsecase>();
		}
		public override void Initialize(AnalysisContext context)
		{
			context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
			context.EnableConcurrentExecution();

			// TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
			// See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information
			context.RegisterCompilationAction(ValidateWork);
		}

		private void ValidateWork(CompilationAnalysisContext context)
		{
			var projectNode = new ProjectNode();

			var result = new List<ClassNode>();
			foreach (var tree in context.Compilation.SyntaxTrees)
			{
				result.AddRange(roslynAdapter.ConvertToGenericTree(tree, context.Compilation.GetSemanticModel(tree)));
			}

			projectNode.ClassNodes = result;
			patternManager.UpdateTree(projectNode);

			return;

			//Temporary code to show adapter is working
			/*foreach (var classnode in projectNode.ClassNodes)
			{
				var test = (ImmutableArray<Location>)classnode.Location;
				foreach (var location in test)
				{
					var diagnostic = Diagnostic.Create(Rule, location: location);
					context.ReportDiagnostic(diagnostic);
				}
			}*/
		}
	}
}
