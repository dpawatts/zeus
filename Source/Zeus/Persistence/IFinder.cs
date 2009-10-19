using System;
using System.Linq;
using Zeus.ContentProperties;

namespace Zeus.Persistence
{
	public interface IFinder
	{
		IQueryable<T> Query<T>();
		IQueryable<T> QueryItems<T>() where T : ContentItem;
		IQueryable<ContentItem> QueryItems();
		IQueryable<PropertyData> QueryDetails();
		IQueryable<T> QueryDetails<T>() where T : PropertyData;
		IQueryable<PropertyCollection> QueryDetailCollections();
		IQueryable Query(Type resultType);
	}
}