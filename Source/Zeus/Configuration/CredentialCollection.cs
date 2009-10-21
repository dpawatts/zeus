using System.Configuration;

namespace Zeus.Configuration
{
	[ConfigurationCollection(typeof(UserElement), AddItemName = "user", CollectionType = ConfigurationElementCollectionType.BasicMapAlternate)]
	public sealed class CredentialCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new UserElement();
		}

		protected override ConfigurationElement CreateNewElement(string elementName)
		{
			return new UserElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			UserElement rule = (UserElement) element;
			return rule.Name;
		}

		protected override bool IsElementName(string elementname)
		{
			return elementname == "user";
		}

		public override ConfigurationElementCollectionType CollectionType
		{
			get { return ConfigurationElementCollectionType.BasicMapAlternate; }
		}

		protected override string ElementName
		{
			get { return "user"; }
		}
	}
}