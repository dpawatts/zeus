using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Isis.ExtensionMethods;
using Isis.Linq;
using NHibernate.Linq;
using Zeus.ContentProperties;
using Zeus.Persistence.Specifications;

namespace Zeus.Persistence.NH
{
	public class ItemFinder : Finder<ContentItem>
	{
		#region Constructor

		public ItemFinder(ISessionProvider sessionProvider)
			: base(sessionProvider)
		{
			
		}

		#endregion
	}
}
