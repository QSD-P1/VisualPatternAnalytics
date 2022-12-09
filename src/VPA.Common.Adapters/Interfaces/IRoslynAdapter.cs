using Microsoft.CodeAnalysis;
using VPA.Domain.Models;

namespace VPA.Common.Adapters.Interfaces
{
	public interface IRoslynAdapter
	{
		 public GenericTree ConvertToGenericTree(SyntaxTree tree);
	}
}
