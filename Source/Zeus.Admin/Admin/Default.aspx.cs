using System;
using Zeus.Configuration;
using System.Configuration;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Zeus.Admin
{
	public partial class Default : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			imgLogo.Visible = !((AdminSection) ConfigurationManager.GetSection("zeus/admin")).HideBranding;
			ltlAdminName.Text = ((AdminSection) ConfigurationManager.GetSection("zeus/admin")).Name;

			foreach (ToolbarPluginAttribute toolbarPlugin in GetToolbarPlugins())
				toolbarPlugin.AddTo(plcToolbar);
		}

		private IEnumerable<ToolbarPluginAttribute> GetToolbarPlugins()
		{
			List<ToolbarPluginAttribute> plugins = new List<ToolbarPluginAttribute>();
			foreach (Assembly assembly in GetAssemblies())
				plugins.AddRange(FindPluginsIn(assembly));
			return plugins;
		}

		private IEnumerable<ToolbarPluginAttribute> FindPluginsIn(Assembly a)
		{
			foreach (ToolbarPluginAttribute attribute in a.GetCustomAttributes(typeof(ToolbarPluginAttribute), false))
				yield return attribute;
			foreach (Type t in a.GetTypes())
			{
				foreach (ToolbarPluginAttribute attribute in t.GetCustomAttributes(typeof(ToolbarPluginAttribute), false))
					yield return attribute;
			}
		}

		// Copied from ContentTypeBuilder
		private string _assemblySkipLoadingPattern = "^System|^mscorlib|^Microsoft|^CppCodeProvider|^VJSharpCodeProvider|^WebDev|^Castle|^Iesi|^log4net|^NHibernate|^nunit|^TestDriven|^MbUnit|^Rhino|^QuickGraph|^TestFu|^SoundInTheory\\.NMigration|^SoundInTheory\\.DynamicImage";

		private IList<Assembly> GetAssemblies()
		{
			List<Assembly> assemblies = new List<Assembly>();

			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				if (Matches(assembly.FullName))
					assemblies.Add(assembly);
			}

			foreach (string dllPath in Directory.GetFiles(HttpContext.Current.Server.MapPath("~/bin"), "*.dll"))
			{
				try
				{
					Assembly assembly = Assembly.ReflectionOnlyLoadFrom(dllPath);
					if (Matches(assembly.FullName) && !assemblies.Any(a => a.FullName == assembly.FullName))
					{
						Assembly loadedAssembly = AppDomain.CurrentDomain.Load(assembly.FullName);
						assemblies.Add(loadedAssembly);
					}
				}
				catch (BadImageFormatException)
				{
					//Trace.TraceError(ex.ToString());
				}
			}

			return assemblies;
		}

		private bool Matches(string assemblyFullName)
		{
			return !Matches(assemblyFullName, _assemblySkipLoadingPattern);
		}

		private bool Matches(string assemblyFullName, string pattern)
		{
			return Regex.IsMatch(assemblyFullName, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
		}
	}
}
