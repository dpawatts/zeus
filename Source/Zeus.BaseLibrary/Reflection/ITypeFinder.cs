using System;
using System.Collections.Generic;

namespace Zeus.BaseLibrary.Reflection
{
	public interface ITypeFinder
	{
		IEnumerable<Type> Find(Type requestedType);
	}
}