using System;
using System.Collections.Generic;
using System.Text;
using VPA.Domain.Enums;
using VPA.Domain.Models;
using VPA.Usecases.DetectionHelpers;
using VPA.Usecases.Extensions;
using VPA.Usecases.Interfaces;

namespace VPA.Usecases.Detectors
{
	public class ProxyDetectorUsecase : IProxyDetectorUsecase
	{
		public ProxyDetectorUsecase() { }
		public string PatternName => "Proxy";

		public async Task<DetectorResultCollection> Detect(ProjectNode project)
		{
			var collection = new DetectorResultCollection()
			{
				Name = PatternName
			};

			foreach (var classNode in project.ClassNodes)
			{
				// we can create these here because 1 singleton can only have 1 related class
				var result = new DetectorResult();
				var itemResult = new DetectedItem();

				if (classNode.Interfaces != null)
				{
					foreach (FieldNode fieldNode in classNode.Children.OfTypeWithAccessModifier<FieldNode>(AccessModifierEnum.Private))
					{
						if (FieldHelper.HasFoundOtherClassFromFieldType(project.ClassNodes, fieldNode, classNode, out var foundClass))
						{
							foreach (string @interface in classNode.Interfaces)
							{
								if (foundClass.Interfaces.Contains(@interface))
								{
									itemResult.MainNode = classNode;
									itemResult.Children.Add(foundClass);
									itemResult.Children.Add(fieldNode);

									//// add items to result;
									result.Items.Add(itemResult);
									collection.Results.Add(result);
								}
							}
							// todo interface here
						}
						//itemResult.MainNode = fieldNode;
						//itemResult.Children.AddRange(leaves);
						//itemResult.Children.Add(fieldLeaf);
						//itemResult.Children.Add(methodLeaf);

						//// add items to result;
						//result.Items.Add(itemResult);
						//collection.Results.Add(result);
					}
				}
			}
			return collection;
		}
	}
}
