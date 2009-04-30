using System.Security.Principal;
using System.Web;
using Zeus.Security;

namespace Zeus.Persistence.Specifications
{
	public class AccessSpecification<T> : Specification<T>
		where T : ContentItem
	{
		public AccessSpecification(string operation)
			: this(HttpContext.Current != null ? HttpContext.Current.User : null, Context.SecurityManager, operation)
		{

		}

		public AccessSpecification(IPrincipal user, ISecurityManager securityManager, string operation)
			: base(ci => securityManager.IsAuthorized(ci, user, operation))
		{
			
		}
	}
}
