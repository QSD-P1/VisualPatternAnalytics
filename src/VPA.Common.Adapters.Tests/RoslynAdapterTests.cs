using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using VPA.Common.Adapters.Adapters;
using VPA.Domain.Enums;
using VPA.Domain.Models;

namespace VPA.Common.Adapters.Tests
{
	public class RoslynAdapterTests : IDisposable
	{
		public RoslynAdapterTests()
		{

		}
		public void Dispose()
		{

		}

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
			var adapter = new RoslynAdapter();

			// Act
			var result = (ClassNode)adapter.ConvertToGenericTree(tree, model).First();

			// Assert
			var expected = new ClassNode
			{
				Name = "MyClass",
				AccessModifiers = AccessModifierEnum.Public,
				Interfaces = new List<string>(),
				ParentClassName = "object",
			};
			Assert.Multiple(
				() => Assert.Equal(expected.Name, actual: result.Name),
				() => Assert.Equal(expected.AccessModifiers, actual: result.AccessModifiers),
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
			var adapter = new RoslynAdapter();

			// Act
			var result = (ConstructorNode)adapter.ConvertToGenericTree(tree, model).First().ChildNodes.First();

			// Assert
			var expected = new ConstructorNode
			{
				Name = "Constructor",
				AccessModifiers = AccessModifierEnum.Public,
				Parameter = new List<string>(),
				ChildNodes = new List<BaseNode>(),
			};
			Assert.Multiple(
				() => Assert.Equal(expected.Name, actual: result.Name),
				() => Assert.Equal(expected.AccessModifiers, actual: result.AccessModifiers),
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
			var adapter = new RoslynAdapter();

			// Act
			var result = (ClassNode)adapter.ConvertToGenericTree(tree, semanticModel).First();

			// Assert
			Assert.Multiple(
				() => Assert.NotNull(result),
				() => Assert.Equal("TestClass", result.Name),
				() => Assert.Equal(AccessModifierEnum.Internal, result.AccessModifiers),
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
			var adapter = new RoslynAdapter();

			// Act
			var result = (ConstructorNode)adapter.ConvertToGenericTree(tree, semanticModel).First().ChildNodes.First();

			// Assert
			Assert.Multiple(
				() => Assert.NotNull(result),
				() => Assert.Equal("Constructor", result.Name),
				() => Assert.Equal(AccessModifierEnum.Public, result.AccessModifiers),
				() => Assert.Equal(new[] { "Int32", "String" }, result.Parameter),
				() => Assert.NotNull(result.Location)
			);
		}
	}
}