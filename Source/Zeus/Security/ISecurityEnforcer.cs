using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeus.Security
{
	public interface ISecurityEnforcer
	{
		/// <summary>
		/// Is invoked when a security violation is encountered. The security 
		/// exception can be cancelled by setting the cancel property on the event 
		/// arguments.
		/// </summary>
		event EventHandler<CancelItemEventArgs> AuthorizationFailed;

		void AuthoriseRequest();
	}
}
