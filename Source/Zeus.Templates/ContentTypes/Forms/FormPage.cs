using Ext.Net;
using Zeus.Design.Editors;
using Zeus.Integrity;
using Zeus.Web;
using Zeus.Web.UI;

namespace Zeus.Templates.ContentTypes.Forms
{
	[ContentType("Form Page", "FormPage", "A page with a form that can be submitted and sent to an email address.", "", 240)]
	[RestrictParents(typeof(WebsiteNode), typeof(Page))]
	[TabPanel("FormPanel", "Form", 100)]
	[DefaultTemplate("Form")]
	public class FormPage : BasePage
	{
		[ChildEditor("Form", 60, ContainerName = "FormPanel")]
		public virtual Form Form
		{
			get { return GetChild("Form") as Form; }
			set
			{
				if (value != null)
				{
					value.Name = "Form";
					value.AddTo(this);
				}
			}
		}

		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Icon.Report); }
		}
	}
}