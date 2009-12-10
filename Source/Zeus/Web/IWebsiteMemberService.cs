using Zeus.Web.Security;

namespace Zeus.Web
{
	public interface IWebsiteMemberService
	{
		TWebsiteMember CreateMember<TWebsiteMember>(WebsiteNode websiteNode, IUser user)
			where TWebsiteMember : WebsiteMember;
		TWebsiteMember GetCurrentMember<TWebsiteMember>(WebsiteNode websiteNode)
			where TWebsiteMember : WebsiteMember;
	}
}