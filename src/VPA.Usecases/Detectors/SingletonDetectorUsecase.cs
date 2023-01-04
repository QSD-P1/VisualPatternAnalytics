using VPA.Domain.Enums;
using VPA.Domain.Models;
using VPA.Usecases.DetectionHelpers;
using VPA.Usecases.Interfaces;
using VPA.Domain.Enums;

namespace VPA.Usecases.Detectors
{
	public class SingletonDetectorUsecase : ISingletonDetectorUsecase
	{
		public SingletonDetectorUsecase()
		{
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
				var detectionResult = new DetectionResult($"Singleton {collection.Results.Count + 1}");
				var itemResult = new DetectedItem();

				if (
					   AccessModifierHelper.AllOfTypeHasAccessModifier<ConstructorNode>(classNode.Children, AccessModifierEnum.Private, out var leaves)
					&& FieldHelper.ClassHasPrivateStaticFieldWithOwnType(classNode, out var fieldLeaf)
					&& MethodHelper.HasSameClassReturnTypeWithKeywords(classNode, publicStaticKeywords, out var methodLeaf)
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