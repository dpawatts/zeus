using System;
using System.Collections.Generic;

namespace Isis.Reflection
{
	public interface ITypeFinder
	{
		IEnumerable<Type> Find(Type requestedType);
	}
}