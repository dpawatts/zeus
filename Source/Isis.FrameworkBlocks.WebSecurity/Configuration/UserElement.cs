using System.Configuration;

namespace Isis.Web.Configuration
{
	public class UserElement : ConfigurationElement
	{
		[ConfigurationProperty("name", IsKey = true)]
		public string Name
		{
			get { return (string) base["name"]; }
			set { base["name"] = value; }
		}

		[ConfigurationProperty("password")]
		public string Password
		{
			get { return (string) base["password"]; }
			set { base["password"] = value; }
		}
	}
}