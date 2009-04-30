using System;
using System.Configuration;
using System.Web;
using Zeus.Configuration;

namespace Zeus.Web
{
	public static class HttpRequestSupport
	{
		public static bool IsSystemDirectory(string urlPath)
		{
			AdminSection adminSection = ConfigurationManager.GetSection("zeus/admin") as AdminSection;
			if (adminSection != null)
			{
				string adminPath = (!adminSection.Path.StartsWith("/")) ? "/" + adminSection.Path : adminSection.Path;
				return (urlPath.StartsWith(adminPath, StringComparison.OrdinalIgnoreCase));
			}
			return false;
		}

		public static bool IsRequestSystemDirectory
		{
			get
			{
				return (((HttpContext.Current != null) && (HttpContext.Current.Request != null)) && IsSystemDirectory(HttpContext.Current.Request.Url.AbsolutePath));
			}
		}
	}
}