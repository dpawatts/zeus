using Zeus.Integrity;
using Zeus.Web;

namespace Zeus.Templates.ContentTypes.Widgets
{
	[ContentType("Login Widget")]
	[AllowedZones(AllowedZones.AllNamed)]
	public class LoginWidget : WidgetContentItem
	{
		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Ext.Net.Icon.Key); }
		}

		[ContentProperty("Forgotten Password Page", 100)]
		public virtual PageContentItem ForgottenPasswordPage
		{
			get { return GetDetail<PageContentItem>("ForgottenPasswordPage", null); }
			set { SetDetail("ForgottenPasswordPage", value); }
		}

		[ContentProperty("Registration Page", 110)]
		public virtual PageContentItem RegistrationPage
		{
			get { return GetDetail<PageContentItem>("RegistrationPage", null); }
			set { SetDetail("RegistrationPage", value); }
		}
	}
}