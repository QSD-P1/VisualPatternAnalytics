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
		// Arrange Set up for testing
		static DefaultConfiguration config = DefaultConfiguration.GetInstance();
		static IProxyDetectorUsecase detector = config.GetService<IProxyDetectorUsecase>();
		static string proxyName = "Proxy";
		static string proxiedName = "Proxied";
		static IList<string> interfaceList = new List<string>() { "Interface" };

		static FieldNode fieldNode = new FieldNode()
		{
			Type = proxiedName,
			AccessModifier = AccessModifierEnum.Private
		};

		ClassNode ProxyClass = new ClassNode()
		{
			Name = proxyName,
			Interfaces = interfaceList,
			Children = new List<BaseLeaf>()
						{
							fieldNode
						},
		};

		ClassNode ProxiedClass = new ClassNode()
		{
			Name = proxiedName,
			Interfaces = interfaceList
		};

		ClassNode NotPartOfProxyClass = new ClassNode()
		{
			Name = proxiedName
		};

		ClassNode EmptyClass = new ClassNode()
		{ };

		[Fact]
		public async Task ProxyDetector_DetectsPattern()
		{
			// Act
			var projectNode = new ProjectNode()
			{
				ClassNodes = new List<ClassNode>() {ProxyClass, ProxiedClass}
			};

			var result = await detector.Detect(projectNode);

			// Assert
			Assert.True(result.Results.Any());
		}

		[Fact]
		public async Task ProxyDetector_SecondClassDifferentInterface()
		{
			// Act
			var projectNode = new ProjectNode()
			{
				ClassNodes = new List<ClassNode>() { EmptyClass, NotPartOfProxyClass }
			};

			var result = await detector.Detect(projectNode);

			// Assert
			Assert.True(result.Results.Count == 0);
		}

		[Fact]
		public async Task ProxyDetector_DetectsPatternWithMultipleClasses()
		{
			// Act
			var projectNode = new ProjectNode()
			{
				ClassNodes = new List<ClassNode>() { ProxyClass, ProxiedClass, NotPartOfProxyClass }
			};

			var result = await detector.Detect(projectNode);

			// Assert
			Assert.True(result.Results.Count == 1);
		}
	}
}
