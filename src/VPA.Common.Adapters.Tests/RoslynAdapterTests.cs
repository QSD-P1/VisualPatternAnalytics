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
			var result = adapter.ConvertToGenericTree(tree, model);

			// Assert
			var expected = new ClassNode
			{
				Name = "MyClass",
				AccessModifiers = AccessModifierEnum.Public,
				Interfaces = new List<string>(),
				ParentClassName = "object",
				Location = new List<Location> { tree.GetRoot().FindToken(6).GetLocation() }
			};
			Assert.Equal(expected, result.First());
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
			var result = adapter.ConvertToGenericTree(tree, model);

			// Assert
			var expected = new ConstructorNode
			{
				Name = "MyClass",
				AccessModifiers = AccessModifierEnum.Public,
				Location = new List<Location> { tree.GetRoot().FindToken(18).GetLocation() },
				Parameter = new List<string>(),
				ChildNodes = new List<BaseNode>(),
			};
			Assert.Equal(expected, actual: result.First().ChildNodes.First());
		}
	}
}