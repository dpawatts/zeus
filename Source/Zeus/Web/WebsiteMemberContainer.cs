using Coolite.Ext.Web;
using Zeus.Integrity;

namespace Zeus.Web
{
	[ContentType("Website Member Container")]
	[RestrictParents(typeof(WebsiteNode))]
	public class WebsiteMemberContainer : ContentItem
	{
		public const string ContainerName = "users";
		public const string ContainerTitle = "Members";

		public WebsiteMemberContainer()
		{
			Name = ContainerName;
			Title = ContainerTitle;
		}

		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Icon.GroupLink); }
		}
	}
}
