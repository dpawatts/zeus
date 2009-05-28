using System;
using System.Web.UI.WebControls;
using Isis.ExtensionMethods.Web.UI;
using Isis.Web;
using Isis.Web.Hosting;
using Zeus.ContentTypes;

[assembly: EmbeddedResourceFile("Zeus.Admin.ViewDetail.aspx", "Zeus.Admin")]
namespace Zeus.Admin
{
	public partial class ViewDetail : AdminPage
	{
		private string Discriminator
		{
			get { return Request.QueryString["discriminator"]; }
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

			base.OnInit(e);
		}

		protected void btnSave_Command(object sender, CommandEventArgs e)
		{
			if (!this.Page.IsValid)
				return;

			zeusItemEditView.Save();

			//  register the script to close the popup
			Page.ClientScript.RegisterStartupScript(typeof(ViewDetail), "closeThickBox", "self.parent.updated();", true);
		}

		protected override void OnPreRender(EventArgs e)
		{
			Page.ClientScript.RegisterCssResource(typeof(ViewDetail), "Zeus.Admin.Assets.Css.edit.css");
			base.OnPreRender(e);
		}
	}
}
