using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VPA.Common.Adapters.Interfaces;
using VPA.Domain.Enums;
using VPA.Domain.Models;

namespace VPA.Common.Adapters.Adapters.Roslyn
{
	public class RoslynAdapter : IRoslynAdapter
	{
		public IEnumerable<ClassNode> ConvertToGenericTree(SyntaxTree tree, SemanticModel semanticModel)
		{
			var nodes = tree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>();
			var result = new List<ClassNode>();

			foreach (var node in nodes)
			{
				result.Add((ClassNode)ConvertToCustomNode(node, semanticModel));
			}

			return result;
		}

		private BaseLeaf ConvertToCustomNode(SyntaxNode node, SemanticModel semanticModel)
		{
			//List mapping specific types to the representing conversion methods
			//Chosen to use a dictionary instead of switch so the code is easier to read with allot of types
			Dictionary<Type, Func<SyntaxNode, SemanticModel, BaseLeaf>> NodeConvertionDictionary = new()
			{
				{ typeof(ClassDeclarationSyntax), ConvertClassDeclarationToClassNode },
				{ typeof(ConstructorDeclarationSyntax), ConvertConstructorDeclarationToConstructorNode },
				{ typeof(FieldDeclarationSyntax), ConvertFieldDeclarationToFieldNode },
				{ typeof(MethodDeclarationSyntax), ConvertMethodDeclarationToMethodNode }
			};

			//If we can not find a suitable conversionMethod, just return
			if (NodeConvertionDictionary.TryGetValue(node.GetType(), out var SpecificConversionMethod) is false)
			{
				return null;
			}

			// Create a new baseNode for the current SyntaxNode
			var customNode = SpecificConversionMethod(node, semanticModel);

			// Recursively convert the children of the SyntaxNode
			if (customNode is BaseNode baseNode)
			{
				var childNodes = node.ChildNodes();
				var childNodesList = new List<BaseLeaf>();
				foreach (var childNode in childNodes)
				{
					var convertedNode = ConvertToCustomNode(childNode, semanticModel);
					if (convertedNode != null)
					{
						childNodesList.Add(convertedNode);
					}
				}
				baseNode.Children = childNodesList;
			}

			return customNode;
		}

		private ClassNode ConvertClassDeclarationToClassNode(SyntaxNode nodeToConvert, SemanticModel semanticModel)
		{
			var roslynNode = (ClassDeclarationSyntax)nodeToConvert;

			// Get the symbol for the class
			var classSymbol = semanticModel.GetDeclaredSymbol(roslynNode);

			// Convert the roslyndata to generic tree ClassNode
			var newNode = new ClassNode()
			{
				Interfaces = classSymbol.AllInterfaces.Select(x => x.Name).ToList(),
				ParentClassName = classSymbol.BaseType.Name,
			};

			newNode = FillBaseLeafData(roslynNode, classSymbol, newNode);

			return newNode;
		}

		private ConstructorNode ConvertConstructorDeclarationToConstructorNode(SyntaxNode nodeToConvert, SemanticModel semanticModel)
		{
			var roslynNode = (ConstructorDeclarationSyntax)nodeToConvert;

			// Get the symbol for the class
			var constructorSymbol = semanticModel.GetDeclaredSymbol(roslynNode);

			// Convert the roslyndata to generic tree ConstructorNode
			var newNode = new ConstructorNode()
			{
				Parameter = constructorSymbol.Parameters.Select(x => x.Type.Name).ToList(),
			};

			newNode = FillBaseLeafData(roslynNode, constructorSymbol, newNode);
			newNode.Name = "Constructor";

			return newNode;
		}

		private FieldNode ConvertFieldDeclarationToFieldNode(SyntaxNode nodeToConvert, SemanticModel semanticModel)
		{
			//We need to get the symbol on a different way because the semanticModel has a minor bug regarding to fields
			var roslynNode = (FieldDeclarationSyntax)nodeToConvert;
			var fieldSymbol = roslynNode.Declaration.Variables.Select(v => semanticModel.GetDeclaredSymbol(v)).FirstOrDefault();

			// Convert the roslyn data to the method specific values
			var newNode = new FieldNode()
			{
				Type = roslynNode.Declaration.Type.ToString(),
			};

			newNode = FillBaseLeafData(roslynNode, fieldSymbol, newNode);

			return newNode;
		}

		private MethodNode ConvertMethodDeclarationToMethodNode(SyntaxNode nodeToConvert, SemanticModel semanticModel)
		{
			var roslynNode = (MethodDeclarationSyntax)nodeToConvert;

			// Get the symbol for the method
			var methodSymbol = semanticModel.GetDeclaredSymbol(roslynNode);

			// Convert the roslyn data to the method specific values
			var newNode = new MethodNode()
			{
				ReturnType = methodSymbol.ReturnType.Name,
				Parameters = methodSymbol.Parameters.Select(x => x.Type.Name).ToList(),
			};

			newNode = FillBaseLeafData(roslynNode, methodSymbol, newNode);

			return newNode;
		}

		private T FillBaseLeafData<T>(MemberDeclarationSyntax nodeToConvert, ISymbol baseSymbol, T baseLeaf) where T : BaseLeaf
		{
			baseLeaf.Name = baseSymbol.Name;
			baseLeaf.AccessModifier = (AccessModifierEnum)Enum.Parse(typeof(AccessModifierEnum), baseSymbol.DeclaredAccessibility.ToString());
			baseLeaf.Modifiers = nodeToConvert.Modifiers.ToModifiers();
			baseLeaf.Location = baseSymbol.Locations;

			return baseLeaf;
		}
	}
}
