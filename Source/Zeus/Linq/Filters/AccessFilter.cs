using System;
using System.Linq;
using System.Security.Principal;
using Zeus.Security;
using System.Web;

namespace Zeus.Linq.Filters
{
	public class AccessFilter : ItemFilter
	{
		public AccessFilter()
			: this(HttpContext.Current != null ? HttpContext.Current.User : null, Context.SecurityManager)
		{
		}

		public AccessFilter(IPrincipal user, ISecurityManager securityManager)
		{
			this.User = user;
			this.SecurityManager = securityManager;
		}

		public IPrincipal User
		{
			get;
			set;
		}

		public ISecurityManager SecurityManager
		{
			get;
			set;
		}

		public override IQueryable<ContentItem> Filter(IQueryable<ContentItem> source)
		{
			return source.Where(ci => this.SecurityManager.IsAuthorized(ci, this.User));
		}
	}
}
