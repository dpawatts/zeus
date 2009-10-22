using System.Collections.Generic;
using System.Reflection;

namespace Zeus.BaseLibrary.Reflection
{
	public interface IAssemblyFinder
	{
		IEnumerable<Assembly> GetAssemblies();
	}
}