using Zeus.Integrity;
using Zeus.Web;
using Zeus.Web.Security;

namespace Zeus.Templates.ContentTypes
{
	[RestrictParents(typeof(ILoginContext))]
	public class RegistrationPageBase : PageContentItem
	{
        public string MonkeyNonStat { get { return "Monkey"; } }

		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Ext.Net.Icon.KeyAdd); }
		}
	}
}