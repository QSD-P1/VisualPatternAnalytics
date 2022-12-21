using System;
using System.Collections.Generic;
using System.Text;
using VPA.Domain.Enums;

namespace VPA.Domain.Models
{
	public class KeywordCollection
	{
		public AccessModifierEnum AccessModifier { get; set; }
		public IEnumerable<ModifiersEnum> Modifiers { get; set; }
	}
}
