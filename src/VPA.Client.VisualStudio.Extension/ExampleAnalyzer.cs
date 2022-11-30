using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Threading;
using VPA.Common.Adapters.Interfaces;
using VPA.Configuration;
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

		private readonly IAnalyzeSingletonUsecase analyzeSingletonUsecase;
		private readonly IRoslynAdapter roslynAdapter;
		public ExampleAnalyzer()
		{
			//Sadly analyzers dont contain MEF so we cant use Dependency injection.
			//We wrote our own singleton that manages the implementations.
			var config = DefaultConfiguration.GetInstance();
			analyzeSingletonUsecase = config.GetService<IAnalyzeSingletonUsecase>();
			roslynAdapter = config.GetService<IRoslynAdapter>();
		}
		public override void Initialize(AnalysisContext context)
		{
			context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
			context.EnableConcurrentExecution();

			// TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
			// See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information
			//context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Method);
			context.RegisterCompilationAction(ValidateWork);
			context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Method);
		}

		private void ValidateWork(CompilationAnalysisContext context)
		{

			analyzeSingletonUsecase.Analyze(new Domain.Models.GenericTree());
			foreach (var item in context.Compilation.SyntaxTrees)
			{
				foreach (var tree in item.GetCompilationUnitRoot(CancellationToken.None).ChildNodesAndTokens())
				{
					//if (tree.IsKind(SyntaxKind.UsingDirective))
					//{
						var diagnostic = Diagnostic.Create(Rule, location: tree.GetLocation());
						context.ReportDiagnostic(diagnostic);
					//}
				}
			}
		}

		private static void AnalyzeSymbol(SymbolAnalysisContext context)
		{
			// TODO: Replace the following code with your own analysis, generating Diagnostic objects for any issues you find
			var namedTypeSymbol = (IMethodSymbol)context.Symbol;

			// Find just those named type symbols with names containing lowercase letters.
			//if (namedTypeSymbol.Name.ToCharArray().Any(char.IsLower))
			//{
				// For all such symbols, produce a diagnostic.
				var diagnostic = Diagnostic.Create(Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);

				context.ReportDiagnostic(diagnostic);
			//}
		}
	}
}
