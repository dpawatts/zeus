using System;
using System.Linq;
using Zeus.Configuration;
using Zeus.Web.Hosting;

namespace Zeus.Admin.Plugins
{
	public abstract class MenuPluginBase
	{
		public abstract string GroupName { get; }
		public abstract int SortOrder { get; }

		protected virtual bool AvailableByDefault
		{
			get { return true; }
		}

		protected virtual string Name
		{
			get { return GetType().Name; }
		}

		public virtual string[] RequiredScripts
		{
			get { return null; }
		}

		protected virtual string RequiredSecurityOperation
		{
			get { return null; }
		}

		public virtual string[] RequiredUserControls
		{
			get { return null; }
		}

		public virtual bool IsApplicable(ContentItem contentItem)
		{
			// Hide, if this plugin is hidden by default and no overrides exist in web.config
			if (AvailableByDefault)
				return true;

			// Now check if any overrides are present in web.config. If this plugin is
			// not specifically enabled, then don't show it.
			AdminSection adminSection = Context.Current.Resolve<AdminSection>();
			if (adminSection == null || adminSection.MenuPlugins == null)
				return false;

			foreach (MenuPluginElement menuPluginElement in adminSection.MenuPlugins)
				if (menuPluginElement.Name == Name && menuPluginElement.RolesArray.Any(r => Context.Current.WebContext.User.IsInRole(r)))
					return true;

			return false;
		}

		public virtual bool IsDefault(ContentItem contentItem)
		{
			return false;
		}

		public virtual bool IsEnabled(ContentItem contentItem)
		{
			if (!string.IsNullOrEmpty(RequiredSecurityOperation))
			{
				// Check if user has permission to use this plugin.
				if (!Context.SecurityManager.IsAuthorized(contentItem, Context.Current.WebContext.User, RequiredSecurityOperation))
					return false;
			}

			return true;
		}

		protected string GetPageUrl(Type type, string resourcePath)
		{
			return Context.Current.Resolve<IEmbeddedResourceManager>().GetServerResourceUrl(type.Assembly, resourcePath);
		}
	}
}