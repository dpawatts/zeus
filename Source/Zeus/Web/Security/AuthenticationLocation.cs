using System;

namespace Zeus.Web.Security
{
	public class AuthenticationLocation : SecurityLocation
	{
		public bool Enabled { get; set; }
		public string Name { get; set; }
		public string LoginUrl { get; set; }
		public string DefaultUrl { get; set; }
		public TimeSpan Timeout { get; set; }
		public string CookieDomain { get; set; }
		public string CookiePath { get; set; }
		public bool RequireSsl { get; set; }
		public bool SlidingExpiration { get; set; }
	}
}