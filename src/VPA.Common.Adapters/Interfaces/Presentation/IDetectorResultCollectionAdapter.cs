using VPA.Domain.Models;

namespace VPA.Common.Adapters.Interfaces.Presentation
{
	public interface IDetectorResultCollectionAdapter
	{
		/**
		 * Adapt the adaptee to something else
		 */
		public dynamic Adapt(DetectorResultCollection resultCollection);
	}
}
