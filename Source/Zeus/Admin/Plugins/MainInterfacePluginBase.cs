using System;
using Coolite.Ext.Web;
using Zeus.Web.Hosting;

namespace Zeus.Admin.Plugins
{
	public abstract class MainInterfacePluginBase : IMainInterfacePlugin
	{
		public virtual string[] RequiredScripts
		{
			get { return null; }
		}

		public virtual string[] RequiredStyles
		{
			get { return null; }
		}

		public virtual string[] RequiredUserControls
		{
			get { return null; }
		}

		public virtual void ModifyInterface(IMainInterface mainInterface)
		{
			
		}

		public virtual void RegisterScripts(ScriptManager scriptManager)
		{
			string[] requiredScripts = RequiredScripts;
			if (requiredScripts != null)
				foreach (string requiredScript in requiredScripts)
					scriptManager.RegisterClientScriptInclude(GetType().FullName, requiredScript);
		}

		public virtual void RegisterStyles(ScriptManager scriptManager)
		{
			string[] requiredStyles = RequiredStyles;
			if (requiredStyles != null)
				foreach (string requiredStyle in requiredStyles)
					scriptManager.RegisterClientStyleInclude(GetType().FullName, requiredStyle);
		}

		protected string GetPageUrl(Type type, string resourcePath)
		{
			return Context.Current.Resolve<IEmbeddedResourceManager>().GetServerResourceUrl(type.Assembly, resourcePath);
		}
	}
}