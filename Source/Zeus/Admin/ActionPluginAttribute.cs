using System;
using System.Reflection;
using System.Web;
using System.Web.Handlers;
using System.Web.UI;
using Isis;
using Isis.Web;
using Isis.Web.Hosting;
using Isis.Web.UI;
using Zeus.Security;
using Zeus.Web;
using IWebContext=Zeus.Web.IWebContext;

namespace Zeus.Admin
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly)]
	public class ActionPluginAttribute : Attribute, IContextAwareAttribute
	{
		#region Constructors

		public ActionPluginAttribute(string name, string text, string operation, string groupName, int sortOrder, string pageResourceAssembly, string pageResourcePath, string querystring, string target, string imageResourceName)
			: this(name, text, operation, groupName, sortOrder)
		{
			PageResourceAssembly = pageResourceAssembly;
			PageResourcePath = pageResourcePath;
			QueryString = querystring;
			Target = target;
			ImageResourceName = imageResourceName;
			Type = UrlType.Path;
		}

		public ActionPluginAttribute(string name, string text, string operation, string groupName, int sortOrder, string javascript, string imageResourceName)
			: this(name, text, operation, groupName, sortOrder)
		{
			Javascript = javascript;
			ImageResourceName = imageResourceName;
			Type = UrlType.Javascript;
		}

		private ActionPluginAttribute(string name, string text, string operation, string groupName, int sortOrder)
		{
			Name = name;
			Text = text;
			GroupName = groupName;
			SortOrder = sortOrder;
			Operation = operation;
		}

		#endregion

		#region Properties

		private Type ContextType { get; set; }
		private UrlType Type { get; set; }

		public string GroupName { get; set; }
		public string Javascript { private get; set; }
		public string ImageResourceName { private get; set; }
		public string Name { get; set; }
		public string Operation { get; set; }
		public string PageResourceAssembly { private get; set; }
		public string PageResourcePath { private get; set; }
		public string QueryString { private get; set; }
		public int SortOrder { get; set; }
		public string Target { get; set; }
		public string Text { get; set; }
		public Type TypeFilter { get; set; }

		#region Generated properties

		public string ImageUrl
		{
			get { return WebResourceUtility.GetUrl(ContextType, ImageResourceName); }
		}

		public string PageUrl
		{
			get
			{
				string result = null;
				switch (Type)
				{
					case UrlType.Path :
						result = Context.Current.Resolve<IAdminManager>().GetEmbeddedResourceUrl(ContextType.Assembly, PageResourcePath);
						result = new Url(result).AppendQuery(QueryString).ToString();
						break;
					case UrlType.Javascript :
						result = "javascript:" + Javascript;
						break;
				}
				return result;
			}
		}

		public string JavascriptEnableCondition
		{
			get;
			set;
		}

		#endregion

		#endregion

		public void SetContext(object context)
		{
			if (context is Type)
				ContextType = (Type) context;
		}

		public virtual ActionPluginState GetState(ContentItem contentItem, IWebContext webContext, ISecurityManager securityManager)
		{
			// Check if plugin applies to specified content type.
			if (TypeFilter != null && !TypeFilter.IsAssignableFrom(contentItem.GetType()))
				return ActionPluginState.Hidden;

			// Check if user has permission to use this plugin.
			if (!securityManager.IsAuthorized(contentItem, webContext.User, Operation))
				return ActionPluginState.Disabled;

			// Otherwise allow the plugin to control the state.
			return GetStateInternal(contentItem, webContext);
		}

		protected virtual ActionPluginState GetStateInternal(ContentItem contentItem, IWebContext webContext)
		{
			return ActionPluginState.Enabled;
		}

		private enum UrlType
		{
			Path,
			Javascript
		}
	}

	public enum ActionPluginState
	{
		Enabled,
		Disabled,
		Hidden
	}
}
