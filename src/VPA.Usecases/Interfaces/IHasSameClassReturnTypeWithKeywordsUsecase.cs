using VPA.Domain.Models;

namespace VPA.Usecases.Interfaces
{
	public interface IHasSameClassReturnTypeWithKeywordsUsecase
	{
		bool Execute(ClassNode classNode, KeywordCollection? keywordCollection, out MethodNode? matchedResult);
	}
}