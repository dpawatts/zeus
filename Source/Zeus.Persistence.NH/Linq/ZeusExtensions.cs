using NHibernate;
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
        public static System.Linq.IQueryable<T> Linq<T>(this ISession session)
        {
            /*
			QueryOptions options = new QueryOptions();
			return new Query<T>(new ZeusQueryProvider(session, options), options);
             */
            return session.Query<T>();
        }
    }
}