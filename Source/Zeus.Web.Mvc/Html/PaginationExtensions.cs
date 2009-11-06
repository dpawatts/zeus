using System.Collections.Specialized;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Zeus.BaseLibrary.Collections.Generic;

namespace Zeus.Web.Mvc.Html
{
	public static class PaginationExtensions
	{
		public static Pager PagerPartShowing(this HtmlHelper helper, IPagination pagination)
		{
			return new Pager(pagination, helper.ViewContext.HttpContext.Request).ShowingOnly();
		}

		public static Pager PagerPartLinks(this HtmlHelper helper, IPagination pagination)
		{
			return new Pager(pagination, helper.ViewContext.HttpContext.Request).LinksOnly();
		}

		/// <summary>
		/// Renders a pager component from an IPagination datasource.
		/// </summary>
		public class Pager
		{
			private readonly IPagination _pagination;
			private readonly HttpRequestBase _request;

			private string _paginationFormat = "<p>Showing {0} - {1} of {2} </p>";
			private string _paginationSingleFormat = "<p>Showing {0} of {1} </p>";
			private string _paginationFirst = "first";
			private string _paginationPrev = "prev";
			private string _paginationNext = "next";
			private string _paginationLast = "last";
			private string _pageQueryName = "p";

			private bool _showingOnly, _linksOnly;

			/// <summary>
			/// Creates a new instance of the Pager class.
			/// </summary>
			/// <param name="pagination">The IPagination datasource</param>
			/// <param name="request">The current HTTP Request</param>
			public Pager(IPagination pagination, HttpRequestBase request)
			{
				_pagination = pagination;
				_request = request;
			}

			/// <summary>
			/// Specifies the query string parameter to use when generating pager links. The default is 'page'
			/// </summary>
			public Pager QueryParam(string queryStringParam)
			{
				_pageQueryName = queryStringParam;
				return this;
			}

			/// <summary>
			/// Specifies the format to use when rendering a pagination containing a single page. 
			/// The default is 'Showing {0} of {1}' (eg 'Showing 1 of 3')
			/// </summary>
			public Pager SingleFormat(string format)
			{
				_paginationSingleFormat = format;
				return this;
			}

			/// <summary>
			/// Specifies the format to use when rendering a pagination containing multiple pages. 
			/// The default is 'Showing {0} - {1} of {2}' (eg 'Showing 1 to 3 of 6')
			/// </summary>
			public Pager Format(string format)
			{
				_paginationFormat = format;
				return this;
			}

			/// <summary>
			/// Text for the 'first' link.
			/// </summary>
			public Pager First(string first)
			{
				_paginationFirst = first;
				return this;
			}

			/// <summary>
			/// Text for the 'prev' link
			/// </summary>
			public Pager Previous(string previous)
			{
				_paginationPrev = previous;
				return this;
			}

			/// <summary>
			/// Text for the 'next' link
			/// </summary>
			public Pager Next(string next)
			{
				_paginationNext = next;
				return this;
			}

			/// <summary>
			/// Text for the 'last' link
			/// </summary>
			public Pager Last(string last)
			{
				_paginationLast = last;
				return this;
			}

			public Pager ShowingOnly()
			{
				_showingOnly = true;
				return this;
			}

			public Pager LinksOnly()
			{
				_linksOnly = true;
				return this;
			}

			public override string ToString()
			{
				if (_pagination.TotalItems == 0)
					return null;

				var builder = new StringBuilder();

				if (!_linksOnly)
					if (_pagination.PageSize == 1)
						builder.AppendFormat(_paginationSingleFormat, _pagination.FirstItem, _pagination.TotalItems);
					else
						builder.AppendFormat(_paginationFormat, _pagination.FirstItem, _pagination.LastItem, _pagination.TotalItems);

				if (!_showingOnly)
				{
					builder.Append("<ul class=\"clearfix\">");

					if (_pagination.HasPreviousPage)
						builder.Append("<li class=\"pagePrev\">" + CreatePageLink(_pagination.PageNumber - 1, "previous") + "</li>");
					else
						builder.Append(""); //builder.Append("<li class=\"pagePrev\">previous</li>");

					for (int i = 1; i <= _pagination.TotalPages; ++i)
					{
						builder.Append("<li");
						if (_pagination.PageNumber == i)
							builder.Append(" class=\"on\"");
						builder.Append(">" + CreatePageLink(i, i.ToString()) + "</li>");
					}

					if (_pagination.HasNextPage)
						builder.Append("<li class=\"pageNext\">" + CreatePageLink(_pagination.PageNumber + 1, "next") + "</li>");
					else
						builder.Append(""); //builder.Append("<li class=\"pageNext\">next</li>");

					builder.Append("</ul>");
				}

				return builder.ToString();
			}

			private string CreatePageLink(int pageNumber, string text)
			{
				string queryString = CreateQueryString(_request.QueryString);
				string filePath = _request.FilePath;
				return string.Format("<a href=\"{0}?{1}={2}{3}\">{4}</a>", filePath, _pageQueryName, pageNumber, queryString, text);
			}

			private static string CreateQueryString(NameValueCollection values)
			{
				var builder = new StringBuilder();

				foreach (string key in values.Keys)
				{
					if (key == "p")
					//Don't re-add any existing 'page' variable to the querystring - this will be handled in CreatePageLink.
					{
						continue;
					}

					foreach (var value in values.GetValues(key))
						builder.AppendFormat("&amp;{0}={1}", key, value);
				}

				return builder.ToString();
			}
		}
	}
}