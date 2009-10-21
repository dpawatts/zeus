using System.Configuration;
using System;
using System.ComponentModel;
using Zeus.Web.Security;

namespace Zeus.Configuration
{
	public class AuthenticationSection : ConfigurationSection
	{
		[StringValidator(MinLength = 1), ConfigurationProperty("defaultUrl", DefaultValue = "default.aspx")]
		public string DefaultUrl
		{
			get { return (string) base["defaultUrl"]; }
			set { base["defaultUrl"] = string.IsNullOrEmpty(value) ? base.Properties["defaultUrl"].DefaultValue : value; }
		}
 
		[ConfigurationProperty("domain", DefaultValue = "")]
		public string Domain
		{
			get { return (string) base["domain"]; }
			set { base["domain"] = value; }
		}

		[ConfigurationProperty("enabled", DefaultValue = true)]
		public bool Enabled
		{
			get { return (bool) base["enabled"]; }
			set { base["enabled"] = value; }
		}

		[StringValidator(MinLength = 1), ConfigurationProperty("loginUrl", DefaultValue = "login.aspx")]
		public string LoginUrl
		{
			get { return (string) base["loginUrl"]; }
			set { base["loginUrl"] = string.IsNullOrEmpty(value) ? base.Properties["loginUrl"].DefaultValue : value; }
		}

		[StringValidator(MinLength = 1), ConfigurationProperty("name", DefaultValue = ".ISISWEBAUTH")]
		public string Name
		{
			get { return (string) base["name"]; }
			set { base["name"] = string.IsNullOrEmpty(value) ? base.Properties["name"].DefaultValue : value; }
		}

		[StringValidator(MinLength = 1), ConfigurationProperty("path", DefaultValue = "/")]
		public string Path
		{
			get { return (string) base["path"]; }
			set { base["path"] = string.IsNullOrEmpty(value) ? base.Properties["path"].DefaultValue : value; }
		}

		[ConfigurationProperty("requireSsl", DefaultValue = false)]
		public bool RequireSsl
		{
			get { return (bool) base["requireSsl"]; }
			set { base["requireSsl"] = value; }
		}

		[ConfigurationProperty("slidingExpiration", DefaultValue = true)]
		public bool SlidingExpiration
		{
			get { return (bool) base["slidingExpiration"]; }
			set { base["slidingExpiration"] = value; }
		}

		[TimeSpanValidator(MinValueString = "00:01:00", MaxValueString = "10675199.02:48:05.4775807"), TypeConverter(typeof(TimeSpanMinutesConverter)), ConfigurationProperty("timeout", DefaultValue = "30")]
		public TimeSpan Timeout
		{
			get { return (TimeSpan) base["timeout"]; }
			set { base["timeout"] = value; }
		}

		[ConfigurationProperty("credentials")]
		public CredentialCollection Credentials
		{
			get { return (CredentialCollection) base["credentials"]; }
			set { base["credentials"] = value; }
		}

		internal AuthenticationLocation ToAuthenticationLocation()
		{
			AuthenticationLocation location = new AuthenticationLocation();
			location.Enabled = Enabled;
			location.CookieDomain = Domain;
			location.CookiePath = Path;
			location.DefaultUrl = DefaultUrl;
			location.LoginUrl = LoginUrl;
			location.Name = Name;
			location.RequireSsl = RequireSsl;
			location.SlidingExpiration = SlidingExpiration;
			location.Timeout = Timeout;
			return location;
		}
	}
}