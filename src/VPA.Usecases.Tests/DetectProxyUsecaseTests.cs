using VPA.Domain.Models;
using VPA.Domain.Enums;
using VPA.Usecases.Detectors;
using VPA.Usecases.DetectionHelpers;

namespace VPA.Usecases.Tests
{
	public class DetectProxyUsecaseTests
	{
		private readonly DetectProxyUsecase detectProxyUsecase;

		public DetectProxyUsecaseTests()
		{
			detectProxyUsecase = new(
				new CheckForSameInterfaceImplementation(),
				new GetClassFromFieldType()
				);
		}

		// Arrange Set up for testing
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
				ClassNodes = new List<ClassNode>() { ProxyClass, ProxiedClass }
			};

			var result = await detectProxyUsecase.Detect(projectNode);

			// Assert
			Assert.True(result.Results.Any());
		}

		[Fact]
		public async Task ProxyDetector_SecondClassDifferentInterface()
		{
			// Act
			var projectNode = new ProjectNode()
			{
				ClassNodes = new List<ClassNode>() { ProxyClass, NotPartOfProxyClass }
			};

			var result = await detectProxyUsecase.Detect(projectNode);

			// Assert
			Assert.True(result.Results.Count == 0);
		}

		[Fact]
		public async Task ProxyDetector_DetectsPatternWithMultipleClasses()
		{
			// Act
			var projectNode = new ProjectNode()
			{
				ClassNodes = new List<ClassNode>() { EmptyClass, ProxyClass, ProxiedClass, NotPartOfProxyClass }
			};

			var result = await detectProxyUsecase.Detect(projectNode);

			// Assert
			Assert.True(result.Results.Count == 1);
		}
	}
}
