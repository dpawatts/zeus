using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Zeus.BaseLibrary.Web;
using Zeus.Configuration;

namespace Zeus.Web
{
	/// <summary>
	/// Reads the configuration to build and maintains knowledge of 
	/// <see cref="Site"/>s in the application
	/// </summary>
	public class Host : IHost
	{
		#region Fields

		private readonly IDictionary<string, Site> _hostToSites;
		private readonly IWebContext _context;
		private IList<Site> _sites;

		#endregion

		#region Constructor

		public Host(IWebContext context, HostSection config)
		{
			_context = context;

			_hostToSites = new Dictionary<string, Site>(StringComparer.OrdinalIgnoreCase);
			_sites = new List<Site>();
			foreach (SiteElement element in config.Sites)
				AddSite(config, element);
			if (!_hostToSites.ContainsKey("*"))
				throw new ConfigurationErrorsException("At least one <site> section must omit the <siteHosts> section, or <add name=\"*\"> to the <siteHosts> section.");
		}

		#endregion

		public string GetLanguageFromHostName()
		{
			if (!_context.Request.Url.IsAbsoluteUri)
				return null;
			string host = _context.Request.Url.Host;
			if (string.IsNullOrEmpty(host))
				return null;
			return CurrentSite.GetHostLanguageMappings()[host];
		}

		private void AddSite(HostSection hostSection, SiteElement element)
		{
			Site site = new Site(hostSection.RootItemID, element.StartPageID, element.SiteHosts);
			site.Wildcards = hostSection.Wildcards || element.Wildcards;
			_sites.Add(site);

			if (element.SiteHosts == null || element.SiteHosts.Count == 0)
				SetFallbackSettings(site);
			else
				foreach (HostNameElement hostName in element.SiteHosts)
				{
					if (_hostToSites.ContainsKey(hostName.Name))
						throw new ConfigurationErrorsException("A host name can occur only once. " + hostName.Name + "in <siteHosts> has already been defined.");

					if (hostName.Name == "*")
						SetFallbackSettings(site);
					else
						_hostToSites.Add(hostName.Name, site);
				}
		}

		private void SetFallbackSettings(Site site)
		{
			if (_hostToSites.ContainsKey("*"))
				throw new ConfigurationErrorsException("Only one <site> section without a <siteHosts> element is allowed. A <siteHosts> <add name=\"*\" /> </siteHosts> is the same as not defining a siteHosts element.");
			_hostToSites.Add("*", site);
		}

		public Site CurrentSite
		{
			get { return GetSite(_context.Url.HostUrl); }
		}

		public IList<Site> Sites
		{
			get { return _sites; }
		}

		public Site GetSite(Url url)
		{
			if (url == null)
				throw new ArgumentNullException("url", "An initialized Url instance is required.");
			if (!url.IsAbsolute)
				throw new ArgumentException("The url must be absolute to use for mapping.", "url");

			return MapHostToSite(url.Authority, true);
		}

		private Site MapHostToSite(string hostName, bool fallback)
		{
			Site site;
			if (_hostToSites.TryGetValue(hostName, out site))
				return site;
			if (!fallback)
				return null;
			return _hostToSites["*"];
		}
	}
}
