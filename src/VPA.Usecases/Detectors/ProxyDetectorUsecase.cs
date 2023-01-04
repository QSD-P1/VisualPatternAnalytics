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

		public async Task<DetectionResultCollection> Detect(ProjectNode project)
		{
			var collection = new DetectionResultCollection(PatternName);

			foreach (var classNode in project.ClassNodes.Where(x => x.Interfaces != null && x.Children != null))
			{
				foreach (FieldNode fieldNode in classNode.Children.OfTypeWithAccessModifier<FieldNode>(AccessModifierEnum.Private))
				{
					if (FieldHelper.HasFoundOtherClassFromFieldType(project.ClassNodes, fieldNode, classNode, out ClassNode foundClassNode) &&
						InterfaceHelper.HasSameInterface(project.InterfaceNodes, classNode, foundClassNode, out InterfaceNode foundInterfaceNode))
					{

						var itemResultfoundInterface = new DetectedItem();
						itemResultfoundInterface.MainNode = foundInterfaceNode;
						itemResultfoundInterface.Children.Add(classNode);
						itemResultfoundInterface.Children.Add(foundClassNode);

						var itemResultClass = new DetectedItem();
						itemResultClass.MainNode = classNode;
						itemResultClass.Children.Add(fieldNode);
						itemResultClass.Children.Add(foundInterfaceNode);

						var itemResultfoundClass = new DetectedItem();
						itemResultfoundClass.MainNode = foundClassNode;
						itemResultfoundClass.Children.Add(foundInterfaceNode);

						var detectionResult = new DetectionResult($"{PatternName} {collection.Results.Count + 1}");
						detectionResult.DetectedItems.Add(itemResultClass);
						detectionResult.DetectedItems.Add(itemResultfoundClass);
						detectionResult.DetectedItems.Add(itemResultfoundInterface);

						// add items to result;
						collection.Results.Add(detectionResult);
						break;
					}
				}
			}
			return collection;
		}
	}
}
