using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Zeus.Security;

namespace Zeus.Linq
{
	public static class ContentItemFilters
	{
		public static IQueryable<T> Visible<T>(this IQueryable<T> source)
			where T : ContentItem
		{
			return source.Where(ci => ci.Visible);
		}

		public static IEnumerable<T> Visible<T>(this IEnumerable<T> source)
			where T : ContentItem
		{
			return source.Where(ci => ci.Visible);
		}

		public static IQueryable<T> InZone<T>(this IQueryable<T> source, string zoneName)
			where T : ContentItem
		{
			return source.Where(ci => ci.ZoneName == zoneName);
		}

		public static IEnumerable<T> InZone<T>(this IEnumerable<T> source, string zoneName)
			where T : ContentItem
		{
			return source.Where(ci => ci.ZoneName == zoneName);
		}

		public static IEnumerable<T> InZones<T>(this IEnumerable<T> source, params string[] zoneNames)
			where T : ContentItem
		{
			return source.Where(ci => zoneNames.Any(zn => ci.ZoneName == zn));
		}

		public static IEnumerable<T> InZones<T>(this IEnumerable<T> source)
			where T : ContentItem
		{
			return source.Where(ci => !string.IsNullOrEmpty(ci.ZoneName));
		}

		public static IQueryable<T> Published<T>(this IQueryable<T> source)
			where T : ContentItem
		{
			return source.Where(ci => (ci.Published.HasValue && ci.Published.Value <= DateTime.Now)
				&& !(ci.Expires.HasValue && ci.Expires.Value < DateTime.Now));
		}

		public static IEnumerable<T> Published<T>(this IEnumerable<T> source)
			where T : ContentItem
		{
			return source.Where(ci => (ci.Published.HasValue && ci.Published.Value <= DateTime.Now)
				&& !(ci.Expires.HasValue && ci.Expires.Value < DateTime.Now));
		}

		public static IQueryable<T> Pages<T>(this IQueryable<T> source)
			where T : ContentItem
		{
			return source.Where(ci => ci.IsPage);
		}

		public static IEnumerable<T> Pages<T>(this IEnumerable<T> source)
			where T : ContentItem
		{
			return source.Where(ci => ci.IsPage);
		}

		public static IEnumerable<T> Authorized<T>(this IEnumerable<T> source, string operation)
			where T : ContentItem
		{
			return source.Authorized(HttpContext.Current != null ? HttpContext.Current.User : null, Context.SecurityManager, operation);
		}

		public static IEnumerable<T> Authorized<T>(this IEnumerable<T> source, IPrincipal user, ISecurityManager securityManager, string operation)
			where T : ContentItem
		{
			return source.Where(ci => securityManager.IsAuthorized(ci, user, operation));
		}

		public static IEnumerable<T> Navigable<T>(this IQueryable<T> source)
			where T : ContentItem
		{
			return source.Pages().Visible().Published().Authorized(Operations.Read);
		}

		public static IEnumerable<T> Navigable<T>(this IEnumerable<T> source)
			where T : ContentItem
		{
			return source.Pages().Visible().Published().Authorized(Operations.Read);
		}
	}
}