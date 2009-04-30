using Zeus.Engine;
using Zeus.Plugin;

namespace Zeus.Admin.RecycleBin
{
	[AutoInitialize]
	public class RecycleBinInitializer : IPluginInitializer
	{
		public void Initialize(ContentEngine engine)
		{
			engine.AddComponent("zeus.recycleBin", typeof(IRecycleBinHandler), typeof(RecycleBinHandler));
			engine.AddComponent("zeus.deleteInterceptor", typeof(DeleteInterceptor));
		}
	}
}