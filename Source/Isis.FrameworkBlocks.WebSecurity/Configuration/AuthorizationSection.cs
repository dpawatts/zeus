using System.Configuration;

namespace Isis.Web.Configuration
{
	public class AuthorizationSection : ConfigurationSection
	{
		[ConfigurationProperty("", IsDefaultCollection = true)]
		public AuthorizationRuleCollection Rules
		{
			get { return (AuthorizationRuleCollection) base[string.Empty]; }
		}
	}
}
