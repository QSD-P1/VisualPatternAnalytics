using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Xml.Linq;
using VPA.Common.Adapters.Interfaces;
using VPA.Domain.Enums;
using VPA.Domain.Models;

namespace VPA.Common.Adapters.Adapters
{
	public class RoslynAdapter : IRoslynAdapter
	{
		public List<BaseNode> ConvertToGenericTree(SyntaxTree tree, SemanticModel semanticModel)
		{
			var nodes = tree.GetRoot().ChildNodes();
			var result = new List<BaseNode>();
			foreach (var node in nodes)
			{
				result.Add(ConvertToCustomTree(node, semanticModel));
			}
			return result;
		}
		private BaseNode ConvertToCustomTree(SyntaxNode node, SemanticModel semanticModel)
		{
			//List mapping specific types to the representing convert methods
			Dictionary<Type, Func<SyntaxNode, SemanticModel, BaseNode>> NodeConvertionDictionary = new()
			{
				{ typeof(ClassDeclarationSyntax), ConvertClassDeclarationToClassNode },
				{ typeof(ConstructorDeclarationSyntax), ConvertConstructorDeclarationToConstructorNode }
			};

			//If we can not find a suitable conversionMethod, just return
			if (NodeConvertionDictionary.TryGetValue(node.GetType(), out var SpecificConversionMethod) is false)
			{
				return null;
			}

			// Create a new baseNode for the current SyntaxNode
			var customNode = SpecificConversionMethod(node, semanticModel);

			// Recursively convert the children of the SyntaxNode
			customNode.ChildNodes = node.ChildNodes().Select(x => ConvertToCustomTree(x, semanticModel)).ToList();

			return customNode;
		}

		private ClassNode ConvertClassDeclarationToClassNode(SyntaxNode nodeToConvert, SemanticModel semanticModel)
		{
			var roslynNode = (ClassDeclarationSyntax)nodeToConvert;

			// Get the symbol for the class
			var classSymbol = semanticModel.GetDeclaredSymbol(roslynNode);

			// Convert the roslyndata to generic tree classNode
			var newNode = new ClassNode()
			{
				Name = classSymbol.Name,
				AccessModifiers = (AccessModifierEnum)Enum.Parse(typeof(AccessModifierEnum), classSymbol.DeclaredAccessibility.ToString()),
				Interfaces = classSymbol.AllInterfaces.Select(x => x.Name).ToList(),
				ParentClassName = classSymbol.BaseType.Name,
				Location = classSymbol.Locations,
			};
			return newNode;
		}

		private ConstructorNode ConvertConstructorDeclarationToConstructorNode(SyntaxNode nodeToConvert, SemanticModel semanticModel)
		{
			// Get the symbol for the class
			var constructorSymbol = semanticModel.GetDeclaredSymbol((ConstructorDeclarationSyntax)nodeToConvert);

			// Convert the roslyndata to generic tree classNode
			var newNode = new ConstructorNode()
			{
				Name = constructorSymbol.Name,
				AccessModifiers = (AccessModifierEnum)Enum.Parse(typeof(AccessModifierEnum), constructorSymbol.DeclaredAccessibility.ToString()),
				Location = constructorSymbol.Locations,
				Parameter = constructorSymbol.Parameters.Select(x => x.Type.Name).ToList(),
			};
			return newNode;
		}
	}
}
