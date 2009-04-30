using System;
using System.Collections.Generic;
using System.Reflection;
using Isis;
using Isis.ComponentModel;

namespace Zeus.Engine
{
	public class PluginFinder<TPlugin> : IPluginFinder<TPlugin>
		where TPlugin : Attribute
	{
		private readonly IAssemblyFinder _assemblyFinder;

		public PluginFinder(IAssemblyFinder assemblyFinder)
		{
			_assemblyFinder = assemblyFinder;
		}

		public IEnumerable<TPlugin> GetPlugins()
		{
			List<TPlugin> plugins = new List<TPlugin>();
			foreach (Assembly assembly in _assemblyFinder.GetAssemblies())
				plugins.AddRange(FindPluginsIn(assembly));
			return plugins;
		}

		private static IEnumerable<TPlugin> FindPluginsIn(Assembly a)
		{
			foreach (TPlugin attribute in a.GetCustomAttributes(typeof(TPlugin), false))
				yield return ApplyContext(a, attribute);
			foreach (Type t in a.GetTypes())
				foreach (TPlugin attribute in t.GetCustomAttributes(typeof(TPlugin), false))
					yield return ApplyContext(t, attribute);
		}

		private static TPlugin ApplyContext(object context, TPlugin pluginAttribute)
		{
			if (pluginAttribute is IContextAwareAttribute)
				((IContextAwareAttribute) pluginAttribute).SetContext(context);
			return pluginAttribute;
		}
	}
}
