namespace VPA.Configuration
{
	public class ServiceConfiguration
	{
		/// <summary>
		/// The type of the service
		/// </summary>
		public Type Type { get; set; } = default!;

		/// <summary>
		/// Bool indicating if a service is presistent (singleton)
		/// </summary>
		public bool IsPresistent { get; set; }
	}
}
