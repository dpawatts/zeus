using System;

namespace Zeus.Security
{
	/// <summary>
	/// Checks against unauthorized requests, and updates of content items.
	/// </summary>
	public class SecurityEnforcer : ISecurityEnforcer
	{
		private ISecurityManager security;
		private Web.IWebContext webContext;

		public SecurityEnforcer(ISecurityManager security, Web.IWebContext webContext)
		{
			this.webContext = webContext;
			this.security = security;
		}

		/// <summary>Checks that the current user is authorized to access the current item.</summary>
		/// <param name="context">The context of the request.</param>
		public virtual void AuthoriseRequest()
		{
			ContentItem item = webContext.CurrentPage;
			if (item != null)
			{
				if (item != null && !security.IsAuthorized(item, webContext.User))
					//throw new PermissionDeniedException(item, webContext.User);
					throw new UnauthorizedAccessException();
			}
		}
	}
}
