using Microsoft.CodeAnalysis;
using VPA.Common.Adapters.Interfaces;
using VPA.Domain.Models;

namespace VPA.Common.Adapters.Adapters
{
	public class RoslynAdapter : IRoslynAdapter
	{
		public GenericTree ConvertToGenericTree(SyntaxTree tree)
		{
			throw new NotImplementedException();
		}
	}
}
