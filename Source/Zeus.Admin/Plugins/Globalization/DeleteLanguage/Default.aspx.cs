using System;
using System.Linq;
using Zeus.BaseLibrary.ExtensionMethods.Web.UI;

namespace Zeus.Admin.Plugins.Globalization.DeleteLanguage
{
	public partial class Default : PreviewFrameAdminPage
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
	}
}