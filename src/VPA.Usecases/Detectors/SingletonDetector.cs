﻿using System;
using System.Threading.Tasks;
using VPA.Domain.Models;
using VPA.Usecases.DetectionHelpers;
using VPA.Usecases.Interfaces;
using VPA.Domain.Enums;
namespace VPA.Usecases.Usecases
{
	public class SingletonDetector : ISingletonDetector
	{
		public SingletonDetector()
		{
		}

		public string PatternName => "Singletons";

		public async Task<DetectorResultCollection> Detect(ProjectNode project)
		{
			var collection = new DetectorResultCollection()
			{
				Name = PatternName,
				Results = new List<DetectorResult>()
			};

			foreach (var classNode in project.ClassNodes)
			{
				// we can create these here because 1 singleton can only have 1 related class
				var result = new DetectorResult();
				var itemResult = new DetectedItem();

				if(
					   AccessModifierHelper.AllTypeOfHasAccessModifier<ConstructorNode>(classNode.Children, AccessModifierEnum.Private, out var leaves)
					&& FieldHelper.ClassHasPublicStaticFieldWithOwnType(classNode, out var fieldLeaf)
					&& MethodHelper.HasSameClassReturnType(classNode, out var methodLeaves)
					)
				{
					itemResult.MainNode = classNode;
					itemResult.Children.AddRange(leaves);
					itemResult.Children.Add(fieldLeaf);
					itemResult.Children.Add(methodLeaves);

					// add items to result;
					result.Items.Add(itemResult);
					collection.Results.Add(result);
				}
			}
			return collection;
		}

		Task<List<DetectorResult>> IDetector.Detect(ProjectNode tree)
		{
			throw new NotImplementedException();
		}
	}
}