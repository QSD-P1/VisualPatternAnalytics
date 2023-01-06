using System;
using System.Collections.Generic;
using System.Text;
using VPA.Domain.Models;

namespace VPA.Usecases.Interfaces
{
	public interface IGetCollectionGenericObjectUsecase
	{
		public CollectionGenericObject Execute(string typeString);
	}
}
