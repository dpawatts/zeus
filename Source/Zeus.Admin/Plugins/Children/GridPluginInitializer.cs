using System.Linq;
using Zeus.BaseLibrary.Reflection;
using Zeus.Engine;
using Zeus.Plugin;

namespace Zeus.Admin.Plugins.Children
{
	[AutoInitialize]
	public class GridPluginInitializer : IPluginInitializer
	{
		public void Initialize(ContentEngine engine)
		{
			engine.Resolve<ITypeFinder>().Find(typeof(IGridToolbarPlugin))
				.Where(t => !t.IsInterface && !t.IsAbstract)
				.ToList()
				.ForEach(t => engine.AddComponent(null, typeof(IGridToolbarPlugin), t));
		}
	}
}