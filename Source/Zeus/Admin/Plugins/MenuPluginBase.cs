using System;
using Zeus.Web.Hosting;

namespace Zeus.Admin.Plugins
{
	public abstract class MenuPluginBase
	{
		public abstract string GroupName { get; }
		public abstract int SortOrder { get; }

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
			return true;
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