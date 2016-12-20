using System.Linq;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Engine;
using NHibernate.Linq;
using Zeus.ContentProperties;

namespace Zeus.Persistence.NH.Linq
{
    public class ZeusQueryProvider : DefaultQueryProvider, IQueryProvider
    {
        public ZeusQueryProvider(ISessionImplementor session)
            : base(session)
        {

        }

        object IQueryProvider.Execute(Expression expression)
        {
            expression = TransformExpression(expression);
            return Execute(expression);
        }

        TResult IQueryProvider.Execute<TResult>(Expression expression)
        {
            expression = TransformExpression(expression);
            return Execute<TResult>(expression);
        }

        private static Expression TransformExpression(Expression expression)
        {
            return expression;
            //return new ContentPropertyVisitor(Context.Current.Resolve<IContentPropertyManager>()).VisitExpression(expression);
        }
    }
}