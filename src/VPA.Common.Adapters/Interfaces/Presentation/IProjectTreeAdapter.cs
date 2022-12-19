using System;
using System.Collections.Generic;
using System.Text;
using VPA.Domain.Models;

namespace VPA.Common.Adapters.Interfaces.Presentation
{
	public interface IProjectTreeAdapter
	{
		/**
		* Set the ProjectNode object thats adapted to something else
		*/
		public void SetAdaptee(ProjectNode projectNode);

		/**
		 * Adapt the adaptee
		 */
		public dynamic Adapt(dynamic source);

	}
}
