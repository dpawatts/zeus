using System;
using Coolite.Ext.Web;
using Zeus.Web.Hosting;

namespace Zeus.Admin
{
	public abstract class GridToolbarPluginBase : IGridToolbarPlugin
	{
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

		public abstract ToolbarButton GetToolbarButton(ContentItem contentItem, GridPanel gridPanel);
		public abstract void ModifyGrid(ToolbarButton button, GridPanel gridPanel);

		protected string GetPageUrl(Type type, string resourcePath)
		{
			return Context.Current.Resolve<IEmbeddedResourceManager>().GetServerResourceUrl(type.Assembly, resourcePath);
		}
	}
}