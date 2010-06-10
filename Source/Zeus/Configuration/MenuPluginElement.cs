using System.Configuration;

namespace Zeus.Configuration
{
	public class MenuPluginElement : ConfigurationElement
	{
		[ConfigurationProperty("name", IsRequired = true, IsKey = true)]
		public string Name
		{
			get { return (string) base["name"]; }
			set { base["name"] = value; }
		}

		[ConfigurationProperty("roles", IsRequired = true)]
		public string Roles
		{
			get { return (string)base["roles"]; }
			set { base["roles"] = value; }
		}

		public string[] RolesArray
		{
			get { return Roles.Split(','); }
		}
	}
}