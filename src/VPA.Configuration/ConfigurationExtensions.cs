using VPA.Common.Adapters.Adapters.Roslyn;
using VPA.Common.Adapters.Interfaces;
using VPA.Usecases.Interfaces;
using VPA.Usecases.Manager;
using VPA.Usecases.Detectors;

namespace VPA.Configuration
{
	public static class ConfigurationExtensions
	{
		/// <summary>
		/// Add the specified types to the dictionary
		/// </summary>
		/// <typeparam name="I">Must be an interface</typeparam>
		/// <typeparam name="C">Must be a class implementing the interface</typeparam>
		/// <param name="services"></param>
		private static void RegisterTransient<I, C>(this Dictionary<Type, ServiceConfiguration> services)
			where I : class
			where C : class
		{
			RegisterService<I, C>(services, false);
		}

		/// <summary>
		/// Add the specified types to the dictionary
		/// </summary>
		/// <typeparam name="I">Must be an interface</typeparam>
		/// <typeparam name="C">Must be a class implementing the interface</typeparam>
		/// <param name="services"></param>
		private static void RegisterSingleton<I, C>(this Dictionary<Type, ServiceConfiguration> services)
			where I : class
			where C : class
		{
			RegisterService<I, C>(services, true);
		}

		private static void RegisterService<I, C>(Dictionary<Type, ServiceConfiguration> services, bool isPresistent)
			where I : class
			where C : class
		{
			var interfaceName = typeof(I).ToString();
			if (typeof(C).GetInterface(interfaceName) is null)
			{
				throw new ArgumentException($"Given class `{typeof(C)}` does not implement given interface `{interfaceName}`");
			}

			var newConfig = new ServiceConfiguration()
			{
				IsPresistent = isPresistent,
				Type = typeof(C),
			};

			services.Add(typeof(I), newConfig);
		}

		public static Dictionary<Type, ServiceConfiguration> RegisterUsecases(this Dictionary<Type, ServiceConfiguration> services)
		{
			services.RegisterTransient<ISingletonDetectorUsecase, SingletonDetectorUsecase>();
			services.RegisterTransient<IProxyDetectorUsecase, ProxyDetectorUsecase>();
			services.RegisterSingleton<IPatternManagerUsecase, PatternManagerUsecase>();
			return services;
		}

		public static Dictionary<Type, ServiceConfiguration> RegisterAdapters(this Dictionary<Type, ServiceConfiguration> services)
		{
			services.RegisterTransient<IRoslynAdapter, RoslynAdapter>();
			return services;
		}
	}
}
