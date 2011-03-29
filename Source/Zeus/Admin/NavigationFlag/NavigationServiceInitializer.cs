using System;
using Zeus.Engine;
using Zeus.Plugin;

namespace Zeus.Admin.NavigationFlag
{
	[AutoInitialize]
	public class NavigationCachingServiceInitializer : IPluginInitializer
	{
		public void Initialize(ContentEngine engine)
		{
            engine.AddComponent("zeus.admin.navigationFlag.navigationCachingService", typeof(NavigationCachingService));
		}
	}
}