using System.Collections.Generic;
using System.Reflection;

namespace Zeus.Web.Hosting
{
	public class ResourceSettings
	{
		public ResourceSettings()
		{
			AssemblyPathPrefixes = new Dictionary<string, Assembly>();
			ClientResourcePrefixes = new Dictionary<Assembly, string>();
		}

		public Dictionary<string, Assembly> AssemblyPathPrefixes { get; private set; }
		public Dictionary<Assembly, string> ClientResourcePrefixes { get; set; }
	}
}