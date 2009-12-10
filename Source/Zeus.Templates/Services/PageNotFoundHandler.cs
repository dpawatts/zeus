using System;
using Ninject;
using Zeus.Admin;
using Zeus.Configuration;
using Zeus.Templates.ContentTypes;
using Zeus.Web;

namespace Zeus.Templates.Services
{
	/// <summary>
	/// Shows a "404" page whenever the requested page is not found.
	/// </summary>
	public class PageNotFoundHandler : IStartable
	{
		private readonly IUrlParser _urlParser;
		private readonly AdminSection _adminConfig;

		public PageNotFoundHandler(IUrlParser urlParser, AdminSection adminConfig)
		{
			_urlParser = urlParser;
			_adminConfig = adminConfig;
		}

		private void OnUrlParserPageNotFound(object sender, PageNotFoundEventArgs e)
		{
			WebsiteNode websiteNode = _urlParser.StartPage as WebsiteNode;
			if (websiteNode != null && websiteNode.PageNotFoundPage != null && !e.Url.StartsWith(_adminConfig.Path + "/", StringComparison.InvariantCultureIgnoreCase))
				e.AffectedItem = websiteNode.PageNotFoundPage;
		}

		public void Start()
		{
			_urlParser.PageNotFound += OnUrlParserPageNotFound;
		}

		public void Stop()
		{
			_urlParser.PageNotFound -= OnUrlParserPageNotFound;
		}
	}
}