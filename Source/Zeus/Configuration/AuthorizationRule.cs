using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;

namespace Zeus.Configuration
{
	public class AuthorizationRule : ConfigurationElement
	{
		private CommaDelimitedStringCollection _roles, _users;

		public AuthorizationRuleAction Action { get; set; }
 
		[TypeConverter(typeof(CommaDelimitedStringCollectionConverter)), ConfigurationProperty("roles")]
		public StringCollection Roles
		{
			get
			{
				if (_roles == null)
				{
					CommaDelimitedStringCollection strings = (CommaDelimitedStringCollection) base["roles"];
					if (strings == null)
						_roles = new CommaDelimitedStringCollection();
					else
						_roles = strings.Clone();
				}
				return _roles;
			}
		}

		[TypeConverter(typeof(CommaDelimitedStringCollectionConverter)), ConfigurationProperty("users")]
		public StringCollection Users
		{
			get
			{
				if (_users == null)
				{
					CommaDelimitedStringCollection strings = (CommaDelimitedStringCollection) base["users"];
					if (strings == null)
						_users = new CommaDelimitedStringCollection();
					else
						_users = strings.Clone();
				}
				return _users;
			}
		}
 

	}
}