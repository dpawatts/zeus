using System.Configuration;

namespace Zeus.Configuration
{
	[ConfigurationCollection(typeof(AuthorizedRoleElement), AddItemName = "add")]
	public class AuthorizedRoleCollection : ConfigurationElementCollection
	{
		protected override string ElementName
		{
			get { return "add"; }
		}

		public void Add(AuthorizedRoleElement authorizedRole)
		{
			BaseAdd(authorizedRole);
		}

		public void Clear()
		{
			BaseClear();
		}

		protected override ConfigurationElement CreateNewElement()
		{
			return new AuthorizedRoleElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((AuthorizedRoleElement)element).Role;
		}
	}
}