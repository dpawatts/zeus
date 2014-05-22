/*

using System.Linq.Expressions;
using NHibernate;
using NHibernate.Linq;
using Zeus.ContentProperties;

namespace Zeus.Persistence.NH.Linq
{
	public class ZeusQueryProvider : NHibernateQueryProvider
	{
		public ZeusQueryProvider(ISession session, QueryOptions queryOptions)
			: base(session, queryOptions)
		{

		}

		public override object Execute(Expression expression)
		{
			expression = new ContentPropertyVisitor(Context.Current.Resolve<IContentPropertyManager>()).Visit(expression);
			return base.Execute(expression);
		}
	}
}
*/