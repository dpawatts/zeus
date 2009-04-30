using System;
using System.Linq;
using Isis.ExtensionMethods.Web.UI;
using Isis.Web.Hosting;
using Zeus.Security;
using Zeus.Web;

[assembly: EmbeddedResourceFile("Zeus.Admin.Globalization.DeleteLanguage.aspx", "Zeus.Admin")]
namespace Zeus.Admin.Globalization
{
	[DeleteLanguageActionPlugin]
	public partial class DeleteLanguage : PreviewFrameAdminPage
	{
		#region Methods

		protected override void OnLoad(EventArgs e)
		{
			Title = "Delete Language for '" + SelectedItem.Title + "'";
			hlCancel.NavigateUrl = CancelUrl();

			if (!IsPostBack)
			{
				ddlLanguages.DataSource = Engine.LanguageManager.GetTranslationsOf(SelectedItem, false).Select(ci => Engine.LanguageManager.GetLanguage(ci.Language));
				ddlLanguages.DataBind();

				ddlLanguages.SelectedValue = SelectedLanguageCode;
			}

			base.OnLoad(e);
		}

		protected void btnDelete_Click(object sender, EventArgs e)
		{
			try
			{
				ContentItem translation = Engine.LanguageManager.GetTranslationDirect(SelectedItem, ddlLanguages.SelectedValue);
				Zeus.Context.Persister.Delete(translation);

				Refresh(SelectedItem, AdminFrame.Both, false);
			}
			catch (Exception ex)
			{
				//Engine.Resolve<IErrorHandler>().Notify(ex);
				csvException.IsValid = false;
				csvException.Text = ex.ToString();
			}
		}

		protected override void OnPreRender(EventArgs e)
		{
			Page.ClientScript.RegisterCssResource(typeof(Default), "Zeus.Admin.Assets.Css.edit.css");
			base.OnPreRender(e);
		}

		#endregion

		public class DeleteLanguageActionPluginAttribute : LanguageActionPluginAttribute
		{
			public DeleteLanguageActionPluginAttribute()
				: base("DeleteLanguage", "Delete Language", 2, "Zeus.Admin.Globalization.DeleteLanguage.aspx", "Zeus.Admin.Resources.world_delete.png")
			{

			}

			protected override ActionPluginState GetStateInternal(ContentItem contentItem, IWebContext webContext)
			{
				// Disable, if there are no translations of the original item.
				if (!Zeus.Context.Current.LanguageManager.GetTranslationsOf(contentItem.TranslationOf ?? contentItem, false).Any())
					return ActionPluginState.Disabled;
				return base.GetStateInternal(contentItem, webContext);
			}
		}
	}
}
