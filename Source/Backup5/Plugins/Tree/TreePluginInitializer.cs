using System.Linq;
using Zeus.BaseLibrary.Reflection;
using Zeus.Engine;
using Zeus.Plugin;

namespace Zeus.Admin.Plugins.Tree
{
	[AutoInitialize]
	public class TreePluginInitializer : IPluginInitializer
	{
		public void Initialize(ContentEngine engine)
		{
			engine.Resolve<ITypeFinder>().Find(typeof(ITreePlugin))
				.Where(t => !t.IsInterface && !t.IsAbstract)
				.ToList()
				.ForEach(t => engine.AddComponent(null, typeof(ITreePlugin), t));
		}
	}
}