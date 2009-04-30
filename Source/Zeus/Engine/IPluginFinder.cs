using System.Collections.Generic;

namespace Zeus.Engine
{
	public interface IPluginFinder<TPlugin>
	{
		IEnumerable<TPlugin> GetPlugins();
	}
}
