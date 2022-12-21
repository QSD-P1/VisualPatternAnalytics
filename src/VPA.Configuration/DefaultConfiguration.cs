using System.Reflection;

namespace VPA.Configuration
{
	public class DefaultConfiguration
	{
		private static DefaultConfiguration? defaultConfiguration;

		private readonly Dictionary<Type, ServiceConfiguration> _Services = new();

		private static Dictionary<Type, object> PresistantServices = new();
		private DefaultConfiguration()
		{
			_Services.RegisterUsecases();
			_Services.RegisterAdapters();
		}

		/// <summary>
		/// Get instance of the DefaultConfiguration
		/// </summary>
		/// <returns>Singleton DefaultConfiguration</returns>
		public static DefaultConfiguration GetInstance() => defaultConfiguration ??= new DefaultConfiguration();

		public T GetService<T>() where T : class
		{
			return (T)GetService(typeof(T));
		}

		private object GetService(Type T)
		{
			if (_Services.TryGetValue(T, out ServiceConfiguration serviceConfiguration) is false ||
				serviceConfiguration.Type is null)
			{
				throw new ArgumentException("Can't initialize given type. Make sure it is added to the config list");
			}

			object? serviceToReturn = null;
			if (serviceConfiguration.IsPresistent)
			{
				PresistantServices.TryGetValue(serviceConfiguration.Type, out serviceToReturn);
			}

			if (serviceToReturn is null)
			{
				serviceToReturn = GenerateService(T, serviceConfiguration);

				if (serviceConfiguration.IsPresistent)
				{
					PresistantServices.Add(serviceConfiguration.Type, serviceToReturn);
				}
			}

			return serviceToReturn;
		}

		private object? GenerateService(Type T, ServiceConfiguration serviceConfiguration)
		{
			var constructor = serviceConfiguration.Type.GetConstructors()[0];
			var parameters = constructor.GetParameters();

			List<object> resolvedParameters = new();
			foreach (var item in parameters)
			{
				object? result = null;
				if (item.ParameterType == T)
				{
					throw new ArgumentException($"Infinite loop detected. {T.Name} has a self reference.");
				}
				else
				{
					result = GetService(item.ParameterType);
				}

				if (result is not null)
				{
					resolvedParameters.Add(result);
				}
				else
				{
					throw new ArgumentException($"Missing required parameter {item.ParameterType.Name} in injection");
				}
			}
			return constructor.Invoke(resolvedParameters.ToArray());
		}
	}
}
