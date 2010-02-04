using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeus.BaseLibrary.ExtensionMethods.Web.UI;
using Zeus.BaseLibrary.Web.UI;
using Zeus.Globalization;
using Zeus.Globalization.ContentTypes;

namespace Zeus.Admin.Plugins.Globalization.LanguageOverview
{
	[ActionPluginGroup("Globalization", 40)]
	public partial class Default : PreviewFrameAdminPage
	{
		#region Methods

		protected override void OnLoad(EventArgs e)
		{
			Title = "Language Overview for '" + SelectedItem.Title + "'";
			CreateTranslationsTable();

			hlCancel.NavigateUrl = CancelUrl();

			base.OnLoad(e);
		}

		private void CreateTranslationsTable()
		{
			IEnumerable<Language> availableLanguages = Engine.Resolve<ILanguageManager>().GetAvailableLanguages();
			CreateHeaderRow(availableLanguages);
			CreateRows(availableLanguages);
		}

		private void CreateHeaderRow(IEnumerable<Language> languages)
		{
			// The columns are the available languages.
			TableHeaderRow headerRow = new TableHeaderRow { CssClass = "titles" };
			headerRow.Cells.Add(new TableHeaderCell { Text = "Page" });
			foreach (Language language in languages)
				headerRow.Cells.Add(new TableHeaderCell { Text = "<img src=\"" + language.IconUrl + "\" /> " + language.Title });
			tblPageTranslations.Rows.Add(headerRow);
		}

		private void CreateRows(IEnumerable<Language> languages)
		{
			CreateRow(SelectedItem, languages, 5);
			foreach (ContentItem child in SelectedItem.GetChildren().Where(ci => Engine.LanguageManager.CanBeTranslated(ci)))
				CreateRow(child, languages, 15);
		}

		private void CreateRow(ContentItem item, IEnumerable<Language> languages, int paddingLeft)
		{
			TableRow row = new TableRow();
			TableCell titleCell = new TableCell { Text = "<a href=\"languageoverview.aspx?selected=" + item.Path + "\">" + item.Title + "</a>" };
			titleCell.Style[HtmlTextWriterStyle.PaddingLeft] = paddingLeft + "px";
			row.Cells.Add(titleCell);
			foreach (Language language in languages)
			{
				string text;
				if (Engine.LanguageManager.TranslationExists(item, language.Name))
					text = string.Format("<img src=\"{0}\" />", Utility.GetCooliteIconUrl(Ext.Net.Icon.Tick));
				else
					text = "Create";
				string link = string.Format("<a href=\"{0}\">{1}</a>", Engine.AdminManager.GetEditExistingItemUrl(item, language.Name), text);
				row.Cells.Add(new TableCell { Text = link });
			}
			tblPageTranslations.Rows.Add(row);
		}

		protected override void OnPreRender(EventArgs e)
		{
			Page.ClientScript.RegisterCssResource(typeof(Default), "Zeus.Admin.Assets.Css.view.css");
			Page.ClientScript.RegisterCssResource(typeof(Default), "Zeus.Admin.Assets.Css.globalization.css");
			base.OnPreRender(e);
		}

		#endregion
	}
}