using Zeus.Configuration;
using Zeus.Engine;
using Zeus.Plugin;

namespace Zeus.Admin.RecycleBin
{
	[AutoInitialize]
	public class RecycleBinInitializer : IPluginInitializer
	{
		public void Initialize(ContentEngine engine)
		{
			if (engine.Resolve<AdminSection>().RecycleBin.Enabled)
			{
				engine.AddComponent("zeus.recycleBin", typeof(IRecycleBinHandler), typeof(RecycleBinHandler));
				engine.AddComponent("zeus.deleteInterceptor", typeof(DeleteInterceptor));
			}
		}
	}
}