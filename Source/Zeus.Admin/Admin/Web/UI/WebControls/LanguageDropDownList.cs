using System;
using System.Web.UI.WebControls;
using Zeus.FileSystem.Images;
using Zeus.Globalization;
using Zeus.Globalization.ContentTypes;

namespace Zeus.Admin.Web.UI.WebControls
{
	public class LanguageDropDownList : DropDownList
	{
		public LanguageDropDownList()
		{
			AutoPostBack = true;
		}

		public event EventHandler<LanguageChangedEventArgs> LanguageChanged;

		protected override void OnLoad(EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				foreach (Language language in Zeus.Context.Current.Resolve<ILanguageManager>().GetAvailableLanguages())
				{
					ListItem listItem = new ListItem(language.Title, language.Name);
					if (language.FlagIcon != null)
						listItem.Attributes["style"] = "background-image: url(" + language.FlagIcon.Url + "); background-repeat: no-repeat; background-position: 2px 2px; padding-left: 20px;";
					Items.Add(listItem);
				}
			}

			base.OnLoad(e);
		}

		protected override void OnSelectedIndexChanged(EventArgs e)
		{
			if (LanguageChanged != null)
				LanguageChanged(this, new LanguageChangedEventArgs { LanguageCode = SelectedValue });
			base.OnSelectedIndexChanged(e);
		}
	}

	public class LanguageChangedEventArgs : EventArgs
	{
		public string LanguageCode { get; set; }
	}
}