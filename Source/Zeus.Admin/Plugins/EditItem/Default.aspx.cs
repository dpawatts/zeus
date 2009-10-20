using System;
using System.Linq;
using Isis.ExtensionMethods.Web.UI;
using Isis.Web;
using Zeus.Admin.Web.UI.WebControls;
using Zeus.Configuration;
using Zeus.ContentTypes;
using Zeus.Security;
using Zeus.Web;
using Zeus.Web.UI.WebControls;

namespace Zeus.Admin.Plugins.EditItem
{
	[AvailableOperation(Operations.Change, "Change", 30)]
	public partial class Default : PreviewFrameAdminPage
	{
		private string Discriminator
		{
			get { return Request.QueryString["discriminator"]; }
		}

		protected override void OnInit(EventArgs e)
		{
			if (Discriminator != null)
			{
				Title = "New " + TypeDefinition.Title;
			}
			else
			{
				if (SelectedLanguageCode != null)
				{
					ContentItem translatedItem = Engine.LanguageManager.GetTranslationDirect(SelectedItem, SelectedLanguageCode);
					if (translatedItem == null)
					{
						Title = string.Format("New Translation of '{0}'", SelectedItem.Title);
						ContentItem selectedItem = Engine.ContentTypes.CreateInstance(SelectedItem.GetType(), SelectedItem.Parent);
						selectedItem.Language = SelectedLanguageCode;
						selectedItem.TranslationOf = SelectedItem;
						SelectedItem.Translations.Add(selectedItem);
						selectedItem.Parent = null;
						zeusItemEditView.CurrentItem = selectedItem;
					}
					else
					{
						zeusItemEditView.CurrentItem = translatedItem;
						Title = "Edit \"" + translatedItem.Title + "\"";
					}
				}
				else
				{
					zeusItemEditView.CurrentItem = SelectedItem;
					Title = "Edit \"" + SelectedItem.Title + "\"";
				}
			}

			hlCancel.NavigateUrl = CancelUrl();
			plcLanguages.Visible = GlobalizationEnabled && Engine.LanguageManager.CanBeTranslated((ContentItem) zeusItemEditView.CurrentItem);

			if (!Engine.Resolve<AdminSection>().Versioning.Enabled || !Engine.SecurityManager.IsAuthorized(SelectedItem, User, Operations.Version))
			{
				btnSaveUnpublished.Visible = false;
				btnPreview.Visible = false;
				btnSave.Text = "Save";
			}

			base.OnInit(e);
		}

		protected override void OnLoad(EventArgs e)
		{
			LoadZones();
			base.OnLoad(e);
		}

		protected void btnSave_Click(object sender, EventArgs e)
		{
			Validate();
			if (!IsValid)
				return;

			try
			{
				SaveChanges();
			}
			catch (Exception ex)
			{
				Engine.Resolve<IErrorHandler>().Notify(ex);
				csvException.IsValid = false;
				csvException.ErrorMessage = ex.ToString();
			}
		}

		private void SaveChanges()
		{
			ItemEditorVersioningMode mode = (((ContentItem) zeusItemEditView.CurrentItem).VersionOf == null) ? ItemEditorVersioningMode.VersionAndSave : ItemEditorVersioningMode.SaveAsMaster;
			if (!Engine.Resolve<AdminSection>().Versioning.Enabled)
				mode = ItemEditorVersioningMode.SaveOnly;
			ContentItem currentItem = (ContentItem) zeusItemEditView.Save((ContentItem) zeusItemEditView.CurrentItem, mode);

			if (Request["before"] != null)
			{
				ContentItem before = Engine.Resolve<Navigator>().Navigate(Request["before"]);
				Engine.Resolve<ITreeSorter>().MoveTo(currentItem, NodePosition.Before, before);
			}
			else if (Request["after"] != null)
			{
				ContentItem after = Engine.Resolve<Navigator>().Navigate(Request["after"]);
				Engine.Resolve<ITreeSorter>().MoveTo(currentItem, NodePosition.After, after);
			}

			Refresh(currentItem.VersionOf ?? currentItem, AdminFrame.Both, false);
			Title = string.Format("'{0}' saved, redirecting...", currentItem.Title);
			zeusItemEditView.Visible = false;
		}

		protected void btnSaveUnpublished_Click(object sender, EventArgs e)
		{
			if (!IsValid)
				return;

			ContentItem savedVersion = SaveVersion();
			string redirectUrl = Engine.AdminManager.GetEditExistingItemUrl(savedVersion, SelectedLanguageCode);
			Response.Redirect(redirectUrl);
		}

		protected void btnPreview_Click(object sender, EventArgs e)
		{
			if (!IsValid)
				return;

			ContentItem savedVersion = SaveVersion();

			Url redirectTo = Engine.AdminManager.GetPreviewUrl(savedVersion);

			redirectTo = redirectTo.AppendQuery("preview", savedVersion.ID);
			if (savedVersion.VersionOf != null)
				redirectTo = redirectTo.AppendQuery("original", savedVersion.VersionOf.ID);
			if (!string.IsNullOrEmpty(Request["returnUrl"]))
				redirectTo = redirectTo.AppendQuery("returnUrl", Request["returnUrl"]);

			Response.Redirect(redirectTo);
		}

		#region Helper methods

		private ContentItem SaveVersion()
		{
			ItemEditorVersioningMode mode = (((ContentItem) zeusItemEditView.CurrentItem).VersionOf == null) ? ItemEditorVersioningMode.VersionOnly : ItemEditorVersioningMode.SaveOnly;
			return (ContentItem) zeusItemEditView.Save((ContentItem) zeusItemEditView.CurrentItem, mode);
		}

		private void LoadZones()
		{
			Type itemType = CurrentItemType;
			uscZones.CurrentItem = (ContentItem) zeusItemEditView.CurrentItem;
			ContentType definition = Zeus.Context.ContentTypes.GetContentType(itemType);
			uscZones.DataSource = definition.AvailableZones;
			uscZones.DataBind();

			hplZones.Visible = definition.AvailableZones.Count > 0;
		}

		private void CheckRelatedVersions(ContentItem item)
		{
			hlNewerVersion.Visible = false;
			hlOlderVersion.Visible = false;

			if (item.VersionOf != null)
			{
				DisplayThisIsVersionInfo(item.VersionOf);
			}
			else if (item.ID > 0)
			{
				var unpublishedVersions = Zeus.Context.Finder.QueryItems().Where(ci => ci.VersionOf == item && ci.Updated > item.Updated)
					.ToList()
					.OrderByDescending(ci => ci.Updated)
					.Take(1);

				if (unpublishedVersions.Any() && unpublishedVersions.First().Updated > item.Updated)
					DisplayThisHasNewerVersionInfo(unpublishedVersions.First());
			}
		}

		private void DisplayThisHasNewerVersionInfo(ContentItem itemToLink)
		{
			string url = Url.ToAbsolute(Engine.AdminManager.GetEditExistingItemUrl(itemToLink, SelectedLanguageCode));
			hlNewerVersion.NavigateUrl = url;
			hlNewerVersion.Visible = true;
		}

		private void DisplayThisIsVersionInfo(ContentItem itemToLink)
		{
			string url = Url.ToAbsolute(Engine.AdminManager.GetEditExistingItemUrl(itemToLink, SelectedLanguageCode));
			hlOlderVersion.NavigateUrl = url;
			hlOlderVersion.Visible = true;
		}

		#endregion

		/// <summary>Gets the type defined by <see cref="TypeDefinition"/>.</summary>
		/// <returns>The item's type.</returns>
		private Type CurrentItemType
		{
			get
			{
				ITypeDefinition contentType = TypeDefinition;
				if (contentType != null)
					return contentType.ItemType;
				return null;
			}
		}

		protected void zeusItemEditView_ItemCreating(object sender, ItemViewEditableObjectEventArgs e)
		{
			if (!string.IsNullOrEmpty(Discriminator))
			{
				ContentItem parentItem = Zeus.Context.Current.Resolve<Navigator>().Navigate(SelectedItem.Path);
				ContentItem contentItem = Zeus.Context.Current.ContentTypes.CreateInstance(CurrentItemType, parentItem);
				contentItem.Language = SelectedLanguageCode;
				contentItem.ZoneName = Page.Request["zoneName"];
				e.AffectedItem = contentItem;
			}
		}

		private ContentType TypeDefinition
		{
			get
			{
				if (!string.IsNullOrEmpty(Discriminator))
					return Zeus.Context.Current.ContentTypes[Discriminator];
				if (SelectedItem != null)
					return Zeus.Context.Current.ContentTypes[SelectedItem.GetType()];
				return null;
			}
		}

		protected void zeusItemEditView_DefinitionCreating(object sender, ItemViewTypeDefinitionEventArgs e)
		{
			e.TypeDefinition = TypeDefinition;
		}

		protected void zeusItemEditView_Saving(object sender, ItemViewEditableObjectEventArgs e)
		{
			//Zeus.Context.Persister.Save((ContentItem) e.AffectedItem);
		}

		protected override void OnPreRender(EventArgs e)
		{
			Page.ClientScript.RegisterCssResource(typeof(Default), "Zeus.Admin.Assets.Css.edit.css");
			CheckRelatedVersions((ContentItem) zeusItemEditView.CurrentItem);
			ddlLanguages.SelectedValue = SelectedLanguageCode;
			base.OnPreRender(e);
		}

		protected void ddlLanguages_LanguageChanged(object sender, LanguageChangedEventArgs e)
		{
			Response.Redirect(new Url(Request.RawUrl).SetQueryParameter("language", e.LanguageCode));
		}
	}
}