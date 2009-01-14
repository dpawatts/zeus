using System;
using NHibernate.Linq;
using NHibernate;
using System.Linq.Expressions;

namespace Zeus.Linq
{
	public class ZeusProvider : NHibernateQueryProvider
	{
		private readonly ISession _session;

		public ZeusProvider(ISession session, QueryOptions queryOptions)
			: base(session, queryOptions)
		{
			
		}

		public override object Execute(Expression expression)
		{
			expression = new DetailPropertyVisitor().Visit(expression);

			return base.Execute(expression);
		}
	}
}
