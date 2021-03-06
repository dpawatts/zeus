using System.Linq;
using Zeus.Admin.Plugins;
using Zeus.BaseLibrary.Reflection;
using Zeus.Engine;
using Zeus.Plugin;

namespace Zeus.Admin
{
	[AutoInitialize]
	public class MainInterfacePluginInitializer : IPluginInitializer
	{
		public void Initialize(ContentEngine engine)
		{
			engine.Resolve<ITypeFinder>().Find(typeof(IMainInterfacePlugin))
				.Where(t => !t.IsInterface && !t.IsAbstract)
				.ToList()
				.ForEach(t => engine.AddComponent(null, typeof(IMainInterfacePlugin), t));
		}
	}
}