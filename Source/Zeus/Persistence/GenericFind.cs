using System;
using System.Collections.Generic;
using Zeus.Collections;

namespace Zeus.Persistence
{
	public abstract class GenericFind<TRoot, TStart>
		where TRoot : ContentItem
		where TStart : ContentItem
	{
		/// <summary>Gets the currently displayed page (based on the query string).</summary>
		public static ContentItem CurrentPage
		{
			get { return Context.CurrentPage; }
		}

		/// <summary>Gets the site's root items.</summary>
		public static TRoot RootItem
		{
			//get { return (TStart) Context.Current.UrlParser.RootItem; }
			get { return (TRoot) Context.Persister.Load(Context.Current.Host.RootItemID); }
		}

		/// <summary>Gets the current start page (this may vary depending on host url).</summary>
		public static TStart StartPage
		{
			get { return (TStart) Context.Current.UrlParser.StartPage; }
		}

		public static IList<ContentItem> ListParents(ContentItem initialItem)
		{
			return new List<ContentItem>(EnumerateParents(initialItem, null, false));
		}

		/// <summary>Enumerates parents of the initial item.</summary>
		/// <param name="initialItem">The page whose parents will be enumerated. The page itself will appear in the enumeration if includeSelf is applied.</param>
		/// <param name="lastAncestor">The last page of the enumeration. The enumeration will contain this page.</param>
		/// <param name="includeSelf">Include the lanitialItem in the enumeration.</param>
		/// <returns>An enumeration of the parents of the initial page. If the last page isn't a parent of the inital page all pages until there are no more parents are returned.</returns>
		public static IEnumerable<ContentItem> EnumerateParents(ContentItem initialItem, ContentItem lastAncestor, bool includeSelf)
		{
			if (initialItem == null) yield break;

			ContentItem item;
			if (includeSelf)
				item = initialItem;
			else if (initialItem != lastAncestor)
				item = initialItem.Parent;
			else
				yield break;

			while (item != null)
			{
				yield return item;
				if (item == lastAncestor)
					break;
				item = item.Parent;
			}
		}
	}
}