using System.Linq.Expressions;
using NHibernate;
using NHibernate.Engine;
using NHibernate.Linq;

namespace Zeus.Persistence.NH.Linq
{
    /// <summary>
    /// Provides a static method that enables LINQ syntax for NHibernate Criteria Queries.
    /// </summary>
    public static class ZeusExtensions
    {
        /// <summary>
        /// Creates a new <see cref="T:NHibernate.Linq.NHibernateQueryProvider"/> object used to evaluate an expression tree.
        /// </summary>
        /// <typeparam name="T">An NHibernate entity type.</typeparam>
        /// <param name="session">An initialized <see cref="T:NHibernate.ISession"/> object.</param>
        /// <returns>An <see cref="T:NHibernate.Linq.NHibernateQueryProvider"/> used to evaluate an expression tree.</returns>
        public static NhQueryable<T> Linq<T>(this ISessionImplementor session)
        {
            NhQueryable<T> intermediate = new NhQueryable<T>(session);
            return new NhQueryable<T>(new ZeusQueryProvider(session), intermediate.Expression);
        }
    }
}