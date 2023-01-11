namespace VPA.Configuration
{
	public class DefaultConfiguration
	{
		private static DefaultConfiguration? defaultConfiguration;

		private readonly Dictionary<Type, ServiceConfiguration> services = new();

		private readonly Dictionary<Type, object> presistantServices = new();
		private DefaultConfiguration()
		{
			services.RegisterUsecases();
			services.RegisterAdapters();
		}

		/// <summary>
		/// Get instance of the DefaultConfiguration
		/// </summary>
		/// <returns>Singleton DefaultConfiguration</returns>
		public static DefaultConfiguration GetInstance() => defaultConfiguration ??= new DefaultConfiguration();

		/// <summary>
		/// Gets the requestes service matching the given type
		/// </summary>
		/// <typeparam name="T">The type of service to get</typeparam>
		/// <returns>The service corresponding to the given type</returns>
		public T GetService<T>() where T : class
		{
			return (T)GetService(typeof(T));
		}

		/// <summary>
		/// Get the given service
		/// </summary>
		/// <param name="T">The type of service to get</param>
		/// <returns>The service corresponding to the given type</returns>
		/// <exception cref="ArgumentException"></exception>
		private object GetService(Type T)
		{
			if (services.TryGetValue(T, out ServiceConfiguration serviceConfiguration) is false ||
				serviceConfiguration.Type is null)
			{
				throw new ArgumentException("Can't initialize given type. Make sure it is added to the config list");
			}

			object? serviceToReturn = null;
			if (serviceConfiguration.IsPresistent)
			{
				presistantServices.TryGetValue(serviceConfiguration.Type, out serviceToReturn);
			}

			if (serviceToReturn is null)
			{
				serviceToReturn = GenerateService(T, serviceConfiguration);

				if (serviceConfiguration.IsPresistent)
				{
					presistantServices.Add(serviceConfiguration.Type, serviceToReturn);
				}
			}

			return serviceToReturn;
		}

		/// <summary>
		/// Generate the requested service with the given ServiceConfiguration
		/// </summary>
		/// <param name="T">The type to generate</param>
		/// <param name="serviceConfiguration">The configuration to use in the generation</param>
		/// <returns>The generated type as object</returns>
		/// <exception cref="ArgumentException"></exception>
		private object GenerateService(Type T, ServiceConfiguration serviceConfiguration)
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
