using System.Configuration;

namespace Zeus.Configuration
{
	public class AuthorizationSection : ConfigurationSection
	{
		[ConfigurationProperty("", IsDefaultCollection = true)]
		public Zeus.Configuration.AuthorizationRuleCollection Rules
		{
			get { return (Zeus.Configuration.AuthorizationRuleCollection) base[string.Empty]; }
		}
	}
}