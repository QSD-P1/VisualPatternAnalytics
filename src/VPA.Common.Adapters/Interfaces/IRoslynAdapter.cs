using Microsoft.CodeAnalysis;
using VPA.Domain.Models;

namespace VPA.Common.Adapters.Interfaces
{
	public interface IRoslynAdapter
	{
		public List<BaseNode> ConvertToGenericTree(SyntaxTree tree, SemanticModel semanticModel);
	}
}
