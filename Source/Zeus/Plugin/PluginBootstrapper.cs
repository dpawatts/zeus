using System;
using System.Collections.Generic;
using System.Reflection;
using Isis.ComponentModel;
using Zeus.Engine;

namespace Zeus.Plugin
{
	/// <summary>
	/// Finds plugins and calls their initializer.
	/// </summary>
	public class PluginBootstrapper : IPluginBootstrapper
	{
		private readonly ITypeFinder _typeFinder;

		public PluginBootstrapper(ITypeFinder typeFinder)
		{
			_typeFinder = typeFinder;
		}

		/// <summary>Gets plugins in the current app domain using the type finder.</summary>
		/// <returns>An enumeration of available plugins.</returns>
		public IEnumerable<IPluginDefinition> GetPluginDefinitions()
		{
			foreach (Type type in _typeFinder.Find(typeof(IPluginInitializer)))
				foreach (AutoInitializeAttribute plugin in type.GetCustomAttributes(typeof(AutoInitializeAttribute), true))
				{
					plugin.InitializerType = type;
					yield return plugin;
				}
		}

		/// <summary>Invokes the initialize method on the supplied plugins.</summary>
		public void InitializePlugins(ContentEngine engine, IEnumerable<IPluginDefinition> plugins)
		{
			List<Exception> exceptions = new List<Exception>();
			foreach (IPluginDefinition plugin in plugins)
			{
				try
				{
					plugin.Initialize(engine);
				}
				catch (Exception ex)
				{
					exceptions.Add(ex);
				}
			}
			if (exceptions.Count > 0)
			{
				string message = "While initializing {0} plugin(s) threw an exception. Please review the stack trace to find out what went wrong.";
				message = string.Format(message, exceptions.Count);

				foreach (Exception ex in exceptions)
					message += Environment.NewLine + Environment.NewLine + "- " + ex.Message;

				throw new PluginInitializationException(message, exceptions.ToArray());
			}
		}
	}
}