using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPA.Domain.Models;
using VPA.Domain.Enums;
using VPA.Configuration;
using VPA.Usecases.Interfaces;

namespace VPA.Usecases.Tests
{
	public class ProxyDetectorUsecaseTests
	{
		[Fact]
		public async Task ProxyDetector_DetectsPattern()
		{
			var config = DefaultConfiguration.GetInstance();
			var detector = config.GetService<IProxyDetectorUsecase>();

			var proxyName = "Proxy";
			var proxiedName = "Proxied";
			IList<string> interfaceList = new List<string>() {"Interface"};

			var fieldNode = new FieldNode()
			{
				Type = proxiedName,
				AccessModifier = AccessModifierEnum.Private
			};

			var classA = new ClassNode()
			{
				Name = proxyName,
				Interfaces = interfaceList,
				Children = new List<BaseLeaf>()
						{
							fieldNode
						},
			};

			var classB = new ClassNode()
			{
				Name = proxiedName,
				Interfaces = interfaceList
			};

			var projectNode = new ProjectNode()
			{
				ClassNodes = new List<ClassNode>() {classA, classB}
			};

			var result = await detector.Detect(projectNode);
			Assert.True(result.Results.Any());
		}
	}
}
