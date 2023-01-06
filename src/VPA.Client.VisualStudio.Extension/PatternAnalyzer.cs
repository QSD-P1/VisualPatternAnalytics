using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Generic;
using System.Collections.Immutable;
using VPA.Common.Adapters.Interfaces;
using VPA.Configuration;
using VPA.Domain.Models;
using VPA.Usecases.Interfaces;

namespace VPA.Client.VisualStudio.Extension
{
	[DiagnosticAnalyzer(LanguageNames.CSharp)]
	public class PatternAnalyzer : DiagnosticAnalyzer
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
		private readonly IManageDesignPatternDetectionUsecase _manageDesignPatternDetection;

		public PatternAnalyzer()
		{
			//Sadly analyzers dont contain MEF so we cant use Dependency injection.
			//We wrote our own singleton that manages the implementations.
			var config = DefaultConfiguration.GetInstance();
			roslynAdapter = config.GetService<IRoslynAdapter>();
			_manageDesignPatternDetection = config.GetService<IManageDesignPatternDetectionUsecase>();
		}
		public override void Initialize(AnalysisContext context)
		{
			context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
			context.EnableConcurrentExecution();
			context.RegisterCompilationAction(ValidateWork);
		}

		private void ValidateWork(CompilationAnalysisContext context)
		{
			var projectNode = new ProjectNode();
			var classResult = new List<ClassNode>();
			var interfaceResult = new List<InterfaceNode>();
			foreach (var tree in context.Compilation.SyntaxTrees)
			{
				var result = roslynAdapter.ConvertToGenericTree(tree, context.Compilation.GetSemanticModel(tree));
				classResult.AddRange(result.ClassNodes);
				interfaceResult.AddRange(result.InterfaceNodes);
			}

			projectNode.ClassNodes = classResult;
			projectNode.InterfaceNodes = interfaceResult;
			_= _manageDesignPatternDetection.UpdateTree(projectNode);
		}
	}
}
