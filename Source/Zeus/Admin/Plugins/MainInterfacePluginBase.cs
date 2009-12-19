using System;
using Coolite.Ext.Web;
using Zeus.Web.Hosting;

namespace Zeus.Admin.Plugins
{
	public abstract class MainInterfacePluginBase : IMainInterfacePlugin
	{
		public virtual string[] RequiredUserControls
		{
			get { return null; }
		}

		public virtual void ModifyInterface(IMainInterface mainInterface)
		{
			
		}

		public virtual void RegisterScripts(ScriptManager scriptManager)
		{
			
		}

		protected string GetPageUrl(Type type, string resourcePath)
		{
			return Context.Current.Resolve<IEmbeddedResourceManager>().GetServerResourceUrl(type.Assembly, resourcePath);
		}
	}
}