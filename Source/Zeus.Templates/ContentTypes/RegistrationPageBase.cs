using Zeus.Integrity;
using Zeus.Web;
using Zeus.Web.Security;

namespace Zeus.Templates.ContentTypes
{
	[RestrictParents(typeof(ILoginContext))]
	public abstract class RegistrationPageBase : PageContentItem
	{
		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Ext.Net.Icon.KeyAdd); }
		}
	}
}