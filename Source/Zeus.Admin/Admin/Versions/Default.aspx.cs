using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Isis.ExtensionMethods.Web.UI;
using Isis.Web;
using Isis.Web.Hosting;
using Zeus.Configuration;
using Zeus.FileSystem.Images;
using Zeus.Globalization.ContentTypes;
using Zeus.Persistence;
using Zeus.Security;

[assembly: EmbeddedResourceFile("Zeus.Admin.Versions.Default.aspx", "Zeus.Admin")]
namespace Zeus.Admin.Versions
{
	[VersioningActionPlugin("Versions", "Versions", 4, "Zeus.Admin.Assets.Images.Icons.book_previous.png")]
	[AvailableOperation(Operations.Version, "Version", 45)]
	public partial class Default : PreviewFrameAdminPage
	{
		#region Fields

		private IEnumerable<ContentItem> _publishedItems;
		private IPersister _persister;
		private IVersionManager _versionManager;

		#endregion

		#region Methods

		protected override void OnInit(EventArgs e)
		{
			Title = "Versions of '" + SelectedItem.Title + "'";

			bool isVersionable = SelectedItem.GetType().GetCustomAttributes(typeof(NotVersionableAttribute), true).Length == 0;
			cvVersionable.IsValid = isVersionable;

			_persister = Zeus.Context.Persister;
			_versionManager = Zeus.Context.Current.Resolve<IVersionManager>();

			chkShowAllLanguages.Visible = GlobalizationEnabled;

			ResetPublishedItems();
			
			base.OnInit(e);
		}

		private void ResetPublishedItems()
		{
			ContentItem publishedItem = SelectedItem.VersionOf ?? SelectedItem;
			if (chkShowAllLanguages.Checked)
			{
				_publishedItems = Engine.LanguageManager.GetTranslationsOf(publishedItem, true);
			}
			else
			{
				ContentItem translatedItem = Engine.LanguageManager.GetTranslationDirect(publishedItem, SelectedLanguageCode);
				if (translatedItem != null)
					publishedItem = translatedItem;
				_publishedItems = new List<ContentItem>(new[] { publishedItem });
			}
		}

		protected void gvHistory_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			int id = Convert.ToInt32(e.CommandArgument);
			ContentItem selectedItem = Engine.Persister.Get(id);
			if (selectedItem.VersionOf == null)
			{
				// do nothing
			}
			else if (e.CommandName == "Publish")
			{
				ContentItem previousVersion = selectedItem;
				ContentItem currentVersion = previousVersion.VersionOf;
				bool deletePrevious = previousVersion.Updated > currentVersion.Updated;
				_versionManager.ReplaceVersion(currentVersion, previousVersion);
				//if (deletePrevious)
				//	_persister.Delete(previousVersion);

				Refresh(currentVersion, AdminFrame.Navigation, false);
				ResetPublishedItems();
			}
			else if (e.CommandName == "Delete")
			{
				_persister.Delete(selectedItem);
			}
		}

		protected override void OnPreRender(EventArgs e)
		{
			Page.ClientScript.RegisterCssResource(typeof(Default), "Zeus.Admin.Assets.Css.view.css");

			gvHistory.DataSource = _publishedItems.SelectMany(ci => _versionManager.GetVersionsOf(ci));
			gvHistory.DataBind();

			hlCancel.NavigateUrl = CancelUrl();

			base.OnPreRender(e);
		}

		protected override string GetPreviewUrl(ContentItem item)
		{
			if (item.VersionOf == null)
				return item.Url;

			return Url.Parse(Engine.AdminManager.GetPreviewUrl(item))
				.AppendQuery("preview", item.ID)
				.AppendQuery("original", item.VersionOf.ID);

			/*return Url.Parse(item.FindPath(PathData.DefaultAction).RewrittenUrl)
				.AppendQuery("preview", item.ID)
				.AppendQuery("original", item.VersionOf.ID);*/
		}

		protected bool IsPublished(object contentItem)
		{
			return _publishedItems.Any(ci => ci == contentItem && ci.VersionOf == null && ci.Published != null);
		}

		protected string GetStatus(ContentItem contentItem)
		{
			if (IsPublished(contentItem))
				return "Published version";

			if (contentItem.Published == null)
				return "Not ready";

			if (contentItem.Published != null && contentItem.Published.Value < DateTime.Now)
				return "Previously published";

			if (contentItem.Published != null && contentItem.Published.Value >= DateTime.Now)
				return "Scheduled publishing";

			return "Unknown";
		}

		#endregion

		protected void gvHistory_RowDeleting(object sender, GridViewDeleteEventArgs e)
		{
			
		}

		protected string GetLanguage(string languageCode)
		{
			Language language = Engine.LanguageManager.GetLanguage(languageCode);
			return "<img src=\"" + language.FlagIcon.Url + "\" /> " + language.Title;
		}

		protected void chkShowAllLanguages_CheckedChanged(object sender, EventArgs e)
		{
			ResetPublishedItems();
		}
	}
}
