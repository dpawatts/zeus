using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Ext.Net;
using Zeus.Admin.Plugins.Tree;
using Zeus.Configuration;
using Zeus.Globalization;
using Zeus.Globalization.ContentTypes;
using Zeus.Web;

namespace Zeus.Admin
{
	public abstract class AdminPage : System.Web.UI.Page
	{
		#region Fields

		private const string REFRESH_NAVIGATION_FORMAT = @"
jQuery(document).ready(function() {{
	if (window.top.zeus)
		window.top.zeus.refreshNavigation('{0}');
	else
		window.location = '{1}';
}});";
		private const string REFRESH_PREVIEW_FORMAT = @"
jQuery(document).ready(function() {{
	if (window.top.zeus)
		window.top.zeus.refreshPreview('{1}');
	else
		window.location = '{1}';
}});";
		private const string REFRESH_BOTH_FORMAT = @"
jQuery(document).ready(function() {{
	if (window.top.zeus)
		window.top.zeus.refresh('{0}', '{1}');
	else
		window.location = '{1}';
}});";

		#endregion

		#region Properties

		protected bool GlobalizationEnabled
		{
			get { return Engine.Resolve<GlobalizationSection>().Enabled; }
		}

		protected virtual string SelectedLanguageCode
		{
			get { return Engine.AdminManager.CurrentAdminLanguageBranch; }
		}

		public virtual ContentItem SelectedItem
		{
			get
			{
				ContentItem selectedItem = GetFromViewState()
					?? GetFromUrl()
					?? Zeus.Context.UrlParser.StartPage;
				return selectedItem;
			}
			set
			{
				if (value != null)
					SelectedItemID = value.ID;
				else
					SelectedItemID = 0;
			}
		}

		protected virtual INode SelectedNode
		{
			get { return SelectedItem; }
		}

		private int SelectedItemID
		{
			get { return (int) (ViewState["SelectedItemID"] ?? 0); }
			set { ViewState["SelectedItemID"] = value; }
		}

		private ContentItem _memorizedItem;
		protected ContentItem MemorizedItem
		{
			get { return _memorizedItem ?? (_memorizedItem = Engine.Resolve<Navigator>().Navigate(Request.QueryString["memory"])); }
		}

		public virtual Engine.ContentEngine Engine
		{
			get { return Zeus.Context.Current; }
		}

		#endregion

		#region Methods

		protected virtual string CancelUrl()
		{
			return Request["returnUrl"] ?? (SelectedItem.VersionOf ?? SelectedNode).PreviewUrl;
		}

		public void Refresh(ContentItem contentItem, AdminFrame frame, bool insideUpdatePanel)
		{
			Refresh(contentItem, frame, insideUpdatePanel, GetPreviewUrl(contentItem));
		}

		public void Refresh(ContentItem contentItem, AdminFrame frame, bool insideUpdatePanel, string url)
		{
			string script = null;
			switch (frame)
			{
				case AdminFrame.Preview:
					script = REFRESH_PREVIEW_FORMAT;
					break;
				case AdminFrame.Navigation:
					script = REFRESH_NAVIGATION_FORMAT;
					break;
				case AdminFrame.Both:
					script = REFRESH_BOTH_FORMAT;
					break;
			}

			// If content item is not visible in tree, then get the first parent item
			// that is visible.
			contentItem = Find.EnumerateParents(contentItem, null, true)
				.First(TreeMainInterfacePlugin.IsVisibleInTree);

			script = string.Format(script,
				contentItem.ID, // 0
				url // 1
			);

			if (ExtNet.IsAjaxRequest)
				ExtNet.ResourceManager.RegisterOnReadyScript(script);
			else if (insideUpdatePanel)
				System.Web.UI.ScriptManager.RegisterStartupScript(
					this, typeof(AdminPage),
					"AddRefreshEditScript",
					script, true);
			else
				ClientScript.RegisterStartupScript(
					typeof(AdminPage),
					"AddRefreshEditScript",
					script, true);
		}

		protected virtual string GetPreviewUrl(ContentItem selectedItem)
		{
			return Request["returnUrl"] ?? Engine.AdminManager.GetPreviewUrl(selectedItem);
		}

		private ContentItem GetFromViewState()
		{
			if (SelectedItemID != 0)
				return Zeus.Context.Persister.Get(SelectedItemID);
			return null;
		}

		private ContentItem GetFromUrl()
		{
			string selected = GetSelectedPath();
			if (!string.IsNullOrEmpty(selected))
				return Engine.Resolve<Navigator>().Navigate(selected);

			string selectedUrl = Request["selectedUrl"];
			if (!string.IsNullOrEmpty(selectedUrl))
				return Engine.UrlParser.Parse(selectedUrl);

			string itemId = Request[PathData.ItemQueryKey];
			if (!string.IsNullOrEmpty(itemId))
				return Engine.Persister.Get(int.Parse(itemId));

			return null;
		}

		protected string GetSelectedPath()
		{
			return Request["selected"];
		}

		#region Error Handling

		protected void SetErrorMessage(BaseValidator validator, Integrity.NameOccupiedException ex)
		{
			Trace.Write(ex.ToString());

			string message = string.Format("An item named '{0}' already exists below '{1}'",
				ex.SourceItem.Name,
				ex.DestinationItem.Name);
			SetErrorMessage(validator, message);
		}

		protected void SetErrorMessage(BaseValidator validator, Integrity.DestinationOnOrBelowItselfException ex)
		{
			Trace.Write(ex.ToString());

			string message = string.Format("Cannot move an item to a destination onto or below itself",
				ex.SourceItem.Name,
				ex.DestinationItem.Name);
			SetErrorMessage(validator, message);
		}

		protected void SetErrorMessage(BaseValidator validator, ContentTypes.NotAllowedParentException ex)
		{
			//Trace.Write(ex.ToString());

			string message = string.Format("The item of type '{0}' isn't allowed below a destination of type '{1}'",
				ex.ContentType.Title,
				Engine.ContentTypes.GetContentType(ex.ParentType).Title);
			SetErrorMessage(validator, message);
		}

		protected static void SetErrorMessage(BaseValidator validator, Exception exception)
		{
			//Engine.Resolve<IErrorHandler>().Notify(exception);
			SetErrorMessage(validator, exception.Message);
		}

		protected static void SetErrorMessage(BaseValidator validator, string message)
		{
			validator.IsValid = false;
			validator.ErrorMessage = message;
		}

		protected static string GetBreadcrumbPath(ContentItem item)
		{
			return item.Path;
			//string breadcrumb = "";
			//for (; item != null; item = item.Parent)
			//    breadcrumb = item.Name + "/" + breadcrumb;
			//return breadcrumb;
		}

		#endregion

		#endregion
	}
}
