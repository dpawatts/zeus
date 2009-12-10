using Coolite.Ext.Web;
using Zeus.Integrity;
using Zeus.Web.Security;

namespace Zeus.Web
{
	[ContentType("Website Member")]
	[RestrictParents(typeof(WebsiteMemberContainer))]
	public class WebsiteMember : ContentItem
	{
		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Icon.UserGo); }
		}

		public override string Title
		{
			get
			{
				if (!string.IsNullOrEmpty(UserIdentifier))
				{
					IUser user = Context.Current.Resolve<ICredentialService>().GetUser(UserIdentifier);
					if (user != null)
						return user.Username;
				}
				return "[Orphaned Member]";
			}
			set { base.Title = value; }
		}

		[ContentProperty("User Identifier", 30)]
		public virtual string UserIdentifier
		{
			get { return GetDetail("UserIdentifier", string.Empty); }
			set { SetDetail("UserIdentifier", value); }
		}
	}
}
