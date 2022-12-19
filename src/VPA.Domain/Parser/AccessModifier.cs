using System;
using System.Collections.Generic;
using System.Text;

namespace VPA.Domain.Parser
{
	[Flags]
	public enum AccessModifier
	{
		@public = 0,
		@private = 1,
		@protected = 2,
		@new = 3,
		@internal = 4,
		@abstract = 5,
		@sealed = 6,
		@static = 7,
		@readonly = 8,
		@volatile = 9,
		@virtual = 10,
		@override = 11,
		@extern = 12,
		@async = 13,
	}
}
