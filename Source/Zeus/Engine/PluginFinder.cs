using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Isis;
using Isis.Reflection;

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
			{
				try
				{
					plugins.AddRange(FindPluginsIn(assembly));
				}
				catch (ReflectionTypeLoadException ex)
				{
					string loaderErrors = string.Empty;
					foreach (Exception loaderEx in ex.LoaderExceptions)
					{
						Trace.TraceError(loaderEx.ToString());
						loaderErrors += ", " + loaderEx.Message;
					}

					throw new ZeusException("Error getting types from assembly " + assembly.FullName + loaderErrors, ex);
				}
			}
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
