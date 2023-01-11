using VPA.Domain.Enums;
using VPA.Domain.Models;
using VPA.Usecases.DetectionHelpers;
using VPA.Usecases.Interfaces;

namespace VPA.Usecases.Detectors
{
	public class DetectSingletonUsecase : IDetectSingletonUsecase
	{
		private readonly IAllOfTypeHasAccessModifierUsecase _allOfTypeHasAccessModifierUsecase;
		private readonly IClassHasPrivateStaticFieldWithOwnTypeUsecase _classHasPrivateStaticFieldWithOwnTypeUsecase;
		private readonly IHasSameClassReturnTypeWithKeywordsUsecase _hasSameClassReturnTypeWithKeywordsUsecase;

		public DetectSingletonUsecase(
			IAllOfTypeHasAccessModifierUsecase allOfTypeHasAccessModifierUsecase,
			IClassHasPrivateStaticFieldWithOwnTypeUsecase classHasPrivateStaticFieldWithOwnTypeUsecase,
			IHasSameClassReturnTypeWithKeywordsUsecase hasSameClassReturnTypeWithKeywordsUsecase)
		{
			_allOfTypeHasAccessModifierUsecase = allOfTypeHasAccessModifierUsecase;
			_classHasPrivateStaticFieldWithOwnTypeUsecase = classHasPrivateStaticFieldWithOwnTypeUsecase;
			_hasSameClassReturnTypeWithKeywordsUsecase = hasSameClassReturnTypeWithKeywordsUsecase;
		}

		public string PatternName => "Singleton";

		public async Task<DetectionResultCollection> Detect(ProjectNode project)
		{
			var collection = new DetectionResultCollection(PatternName);

			var publicStaticKeywords = new KeywordCollection()
			{
				AccessModifier = AccessModifierEnum.Public,
				Modifiers = new[] {
					ModifiersEnum.Static
				}
			};

			foreach (ClassNode classNode in project.ClassNodes)
			{
				// Skip class because no constructors means that it has a single public constructor
				if (!classNode.Children.OfType<ConstructorNode>().Any()) continue;

				// we can create these here because 1 singleton can only have 1 related class
				var detectionResult = new DetectionResult($"{PatternName} {collection.Results.Count + 1}");
				var itemResult = new DetectedItem();

				if (
					   _allOfTypeHasAccessModifierUsecase.Execute<ConstructorNode>(classNode.Children, AccessModifierEnum.Private, out var leaves)
					&& _classHasPrivateStaticFieldWithOwnTypeUsecase.Execute(classNode, out var fieldLeaf)
					&& _hasSameClassReturnTypeWithKeywordsUsecase.Execute(classNode, publicStaticKeywords, out var methodLeaf)
					)
				{
					itemResult.MainNode = classNode;
					itemResult.Children.AddRange(leaves);
					itemResult.Children.Add(fieldLeaf);
					itemResult.Children.Add(methodLeaf);

					// add items to result;
					//We can add the result and item in the same loop since singletons always are just 1 class
					detectionResult.DetectedItems.Add(itemResult);
					collection.Results.Add(detectionResult);
				}
			}
			return collection;
		}
	}
}