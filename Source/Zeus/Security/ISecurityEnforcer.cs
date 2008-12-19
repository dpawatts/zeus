using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeus.Security
{
	public interface ISecurityEnforcer
	{
		void AuthoriseRequest();
	}
}
