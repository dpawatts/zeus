using System;
using System.Linq;
using System.Reflection;
using Zeus.ContentProperties;
using Zeus.Linq;
using Zeus.Persistence.NH.Linq;

namespace Zeus.Persistence.NH
{
	public class Finder : IFinder
	{
		#region Fields

		private readonly ISessionProvider _sessionProvider;
		private readonly IContentPropertyManager _contentPropertyManager;

		#endregion

		#region Constructor

		public Finder(ISessionProvider sessionProvider, IContentPropertyManager contentPropertyManager)
		{
			_sessionProvider = sessionProvider;
			_contentPropertyManager = contentPropertyManager;
		}

		#endregion

		#region IFinder<T> Members

		public IQueryable<T> Query<T>()
		{
			return _sessionProvider.OpenSession.Session.Linq<T>();
		}

		public IQueryable<T> QueryItems<T>()
			where T : ContentItem
		{
			return Query<T>();
		}

		public IQueryable<ContentItem> QueryItems()
		{
			return Query<ContentItem>();
		}

		public IQueryable<PropertyData> QueryDetails()
		{
			return QueryDetails<PropertyData>();
		}

		public IQueryable<T> QueryDetails<T>()
			where T : PropertyData
		{
			return Query<T>();
		}

		public IQueryable<PropertyCollection> QueryDetailCollections()
		{
			return Query<PropertyCollection>();
		}

		public IQueryable Query(Type resultType)
		{
			MethodInfo genericQueryMethod = GetType().GetMethod("Query", Type.EmptyTypes).MakeGenericMethod(resultType);
			return (IQueryable)genericQueryMethod.Invoke(this, null);
		}

		#endregion
	}
}
