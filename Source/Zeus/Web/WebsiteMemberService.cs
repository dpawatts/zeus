using System;
using System.Linq;
using Zeus.ContentTypes;
using Zeus.Web.Security;

namespace Zeus.Web
{
	public class WebsiteMemberService : IWebsiteMemberService
	{
		private readonly IContentTypeManager _contentTypeManager;
		private readonly IWebContext _webContext;

		public WebsiteMemberService(IContentTypeManager contentTypeManager,
			IWebContext webContext)
		{
			_contentTypeManager = contentTypeManager;
			_webContext = webContext;
		}

		public TWebsiteMember CreateMember<TWebsiteMember>(WebsiteNode websiteNode, IUser user)
			where TWebsiteMember : WebsiteMember
		{
			var container = GetWebsiteMemberContainer(websiteNode);
			var websiteMember = _contentTypeManager.CreateInstance<TWebsiteMember>(container);
			websiteMember.UserIdentifier = user.Identifier;
			return websiteMember;
		}

		public TWebsiteMember GetCurrentMember<TWebsiteMember>(WebsiteNode websiteNode)
			where TWebsiteMember : WebsiteMember
		{
			if (!_webContext.User.Identity.IsAuthenticated)
				return null;

			var container = GetWebsiteMemberContainer(websiteNode);
			return container.GetChildren<WebsiteMember>().Cast<TWebsiteMember>()
				.SingleOrDefault(wm => wm.UserIdentifier == _webContext.User.Identity.Name);
		}

		private static WebsiteMemberContainer GetWebsiteMemberContainer(WebsiteNode websiteNode)
		{
			return websiteNode.GetChildren<WebsiteMemberContainer>().FirstOrDefault();
		}
	}
}