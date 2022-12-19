using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using VPA.Common.Adapters.Adapters;
using VPA.Domain.Enums;
using VPA.Domain.Models;

namespace VPA.Common.Adapters.Tests
{
	public class RoslynAdapterTests
	{
		[Fact]
		public void ConvertToGenericTree_ReturnsCorrectClassNode()
		{
			// Arrange
			var tree = CSharpSyntaxTree.ParseText(@"
                public class MyClass
                {
                }
            ");
			var compilation = CSharpCompilation.Create("MyCompilation")
				.AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
				.AddSyntaxTrees(tree);
			var model = compilation.GetSemanticModel(tree);

			var roslynAdapter = new RoslynAdapter();

			// Act
			var result = roslynAdapter.ConvertToGenericTree(tree, model).ClassNodes.First();

			// Assert
			var expected = new ClassNode
			{
				Name = "MyClass",
				AccessModifier = AccessModifierEnum.Public,
				Interfaces = new List<string>(),
				ParentClassName = "object",
			};
			Assert.Multiple(
				() => Assert.Equal(expected.Name, actual: result.Name),
				() => Assert.Equal(expected.AccessModifier, actual: result.AccessModifier),
				() => Assert.Equal(expected.Interfaces, actual: result.Interfaces),
				() => Assert.NotNull(result.Location)
				);
		}

		[Fact]
		public void ConvertToCustomTree_ReturnsCorrectConstructorNode()
		{
			// Arrange
			var tree = CSharpSyntaxTree.ParseText(@"
				public class MyClass
				{
				    public MyClass()
				    {
				    }
				}
			");
			var compilation = CSharpCompilation.Create("MyCompilation")
				.AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
				.AddSyntaxTrees(tree);
			var model = compilation.GetSemanticModel(tree);

			var roslynAdapter = new RoslynAdapter();

			// Act
			var result = (ConstructorNode)roslynAdapter.ConvertToGenericTree(tree, model).ClassNodes.First().ChildNodes.First();

			// Assert
			var expected = new ConstructorNode
			{
				AccessModifier = AccessModifierEnum.Public,
				Parameter = new List<string>(),
				ChildNodes = new List<BaseNode>(),
			};
			Assert.Multiple(
				() => Assert.Equal(expected.Name, actual: result.Name),
				() => Assert.Equal(expected.AccessModifier, actual: result.AccessModifier),
				() => Assert.Equal(expected.Parameter, actual: result.Parameter),
				() => Assert.Equal(expected.ChildNodes, actual: result.ChildNodes),
				() => Assert.NotNull(result.Location)
				);
		}

		[Fact]
		public void ConvertClassDeclarationToClassNode_WithValidInput_ReturnsExpectedResult()
		{
			// Arrange
			var tree = SyntaxFactory.ParseSyntaxTree("class TestClass : BaseClass, ITestInterface { }");
			var compilation = CSharpCompilation.Create("MyCompilation")
				.AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
				.AddSyntaxTrees(tree);
			var semanticModel = compilation.GetSemanticModel(tree);
			var roslynAdapter = new RoslynAdapter();

			// Act
			var result = roslynAdapter.ConvertToGenericTree(tree, semanticModel).ClassNodes.First();

			// Assert
			Assert.Multiple(
				() => Assert.NotNull(result),
				() => Assert.Equal("TestClass", result.Name),
				() => Assert.Equal(AccessModifierEnum.Internal, result.AccessModifier),
				() => Assert.Equal(new[] { "ITestInterface" }, result.Interfaces),
				() => Assert.Equal("BaseClass", result.ParentClassName),
				() => Assert.NotNull(result.Location)
			);
		}

		[Fact]
		public void ConvertConstructorDeclarationToConstructorNode_WithValidInput_ReturnsExpectedResult()
		{
			// Arrange
			var tree = SyntaxFactory.ParseSyntaxTree("class TestClass { public TestClass(int arg1, string arg2) { } }");
			var compilation = CSharpCompilation.Create("MyCompilation")
				.AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
				.AddSyntaxTrees(tree);
			var semanticModel = compilation.GetSemanticModel(tree);

			var roslynAdapter = new RoslynAdapter();

			// Act
			var result = (ConstructorNode)roslynAdapter.ConvertToGenericTree(tree, semanticModel).ClassNodes.First().ChildNodes.First();

			// Assert
			Assert.Multiple(
				() => Assert.NotNull(result),
				() => Assert.Equal("Constructor", result.Name),
				() => Assert.Equal(AccessModifierEnum.Public, result.AccessModifier),
				() => Assert.Equal(new[] { "Int32", "String" }, result.Parameter),
				() => Assert.NotNull(result.Location)
			);
		}

		[Fact]
		public void ConvertFieldDeclarationToFieldNode_ReturnsCorrectFieldNode()
		{
			// Arrange
			var tree = SyntaxFactory.ParseSyntaxTree("class TestClass { private int field1; protected string field2; }");
			var compilation = CSharpCompilation.Create("MyCompilation")
				.AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
				.AddSyntaxTrees(tree);
			var semanticModel = compilation.GetSemanticModel(tree);

			var roslynAdapter = new RoslynAdapter();

			// Act
			var fieldNodes = roslynAdapter.ConvertToGenericTree(tree, semanticModel).ClassNodes.First().ChildNodes
				.OfType<FieldNode>()
				.ToList();

			// Assert
			Assert.Multiple(
				() => Assert.Equal(2, fieldNodes.Count),
				() => Assert.Equal("field1", fieldNodes[0].Name),
				() => Assert.Equal("int", fieldNodes[0].Type),
				() => Assert.Equal("Private", fieldNodes[0].AccessModifier.ToString()),
				() => Assert.Equal("field2", fieldNodes[1].Name),
				() => Assert.Equal("string", fieldNodes[1].Type),
				() => Assert.Equal("Protected", fieldNodes[1].AccessModifier.ToString())
			);
		}


		[Fact]
		public void ConvertMethodDeclarationToMethodNode_ReturnsCorrectMethodNode()
		{
			// Arrange
			var tree = SyntaxFactory.ParseSyntaxTree("class TestClass { private int Method1(int x, string y) { return x; } protected string Method2() { return \"\"; } }");
			var compilation = CSharpCompilation.Create("MyCompilation")
				.AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
				.AddSyntaxTrees(tree);
			var semanticModel = compilation.GetSemanticModel(tree);

			var roslynAdapter = new RoslynAdapter();

			// Act
			var methodNodes = roslynAdapter.ConvertToGenericTree(tree, semanticModel).ClassNodes.First().ChildNodes
				.OfType<MethodNode>()
				.ToList();

			// Assert
			Assert.Multiple(
				() => Assert.Equal(2, methodNodes.Count),
				() => Assert.Equal("Method1", methodNodes[0].Name),
				() => Assert.Equal("Int32", methodNodes[0].ReturnType),
				() => Assert.Equal("Private", methodNodes[0].AccessModifier.ToString()),
				() => Assert.Equal(new List<string> { "Int32", "String" }, methodNodes[0].Parameters),
				() => Assert.Equal("Method2", methodNodes[1].Name),
				() => Assert.Equal("String", methodNodes[1].ReturnType),
				() => Assert.Equal("Protected", methodNodes[1].AccessModifier.ToString())
			);
		}
	}
}