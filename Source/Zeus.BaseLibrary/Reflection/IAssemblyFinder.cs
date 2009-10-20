using System.Collections.Generic;
using System.Reflection;

namespace Isis.Reflection
{
	public interface IAssemblyFinder
	{
		IEnumerable<Assembly> GetAssemblies();
	}
}