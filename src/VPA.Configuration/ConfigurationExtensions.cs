using VPA.Common.Adapters.Adapters;
using VPA.Common.Adapters.Interfaces;
using VPA.Usecases.Interfaces;
using VPA.Usecases.Usecases;

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
		private static void Register<I, C>(this Dictionary<Type, Type> services)
			where I : class
			where C : class
		{
			var interfaceName = typeof(I).ToString();
			if (typeof(C).GetInterface(interfaceName) is null)
			{
				throw new ArgumentException($"Given class `{typeof(C)}` does not implement given interface `{interfaceName}`");
			}

			services.Add(typeof(I), typeof(C));
		}

		public static Dictionary<Type, Type> RegisterUsecases(this Dictionary<Type, Type> services)
		{
			services.Register<IAnalyzeSingletonUsecase, AnalyzeSingletonUsecase>();
			services.Register<IAnalyzeFactoryUsecase, AnalyzeFactoryUsecase>();
			return services;
		}

		public static Dictionary<Type, Type> RegisterAdapters(this Dictionary<Type, Type> services)
		{
			services.Register<IRoslynAdapter, RoslynAdapter>();
			return services;
		}
	}
}
