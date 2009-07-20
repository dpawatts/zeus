using System.Configuration;

namespace Zeus.Configuration
{
	public class AuthorizedRoleElement : ConfigurationElement
	{
		[ConfigurationProperty("role", IsRequired = true, IsKey = true)]
		public string Role
		{
			get { return (string)base["role"]; }
			set { base["role"] = value; }
		}
	}
}