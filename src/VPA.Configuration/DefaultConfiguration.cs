namespace VPA.Configuration
{
	public class DefaultConfiguration
	{
		private static DefaultConfiguration? defaultConfiguration;

		private readonly Dictionary<Type, Type> _Services = new();
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
			if (_Services.TryGetValue(T, out Type serviceType) is false)
			{
				throw new ArgumentException("Can't initialize given type. Make sure it is added to the config list");
			}

			var constructor = serviceType.GetConstructors()[0];
			var parameters = constructor.GetParameters();

			List<object> resolvedParameters = new();

			foreach (var item in parameters)
			{
				object? result = null;
				if(item.ParameterType == T)
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

			var service = constructor.Invoke(resolvedParameters.ToArray());
			return service;
		}
	}
}
