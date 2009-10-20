using System.Linq;
using Isis.Reflection;
using Zeus.Engine;
using Zeus.Plugin;

namespace Zeus.Admin.Plugins.ContextMenu
{
	[AutoInitialize]
	public class ContextMenuPluginInitializer : IPluginInitializer
	{
		public void Initialize(ContentEngine engine)
		{
			engine.Resolve<ITypeFinder>().Find(typeof(IContextMenuPlugin))
				.Where(t => !t.IsInterface && !t.IsAbstract)
				.ToList()
				.ForEach(t => engine.AddComponent(null, typeof(IContextMenuPlugin), t));
		}
	}
}