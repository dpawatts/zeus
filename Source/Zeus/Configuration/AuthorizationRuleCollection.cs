using System.Configuration;
using System.Globalization;

namespace Zeus.Configuration
{
	[ConfigurationCollection(typeof(Zeus.Configuration.AuthorizationRule), AddItemName = "allow,deny", CollectionType = ConfigurationElementCollectionType.BasicMapAlternate)]
	public sealed class AuthorizationRuleCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new Zeus.Configuration.AuthorizationRule();
		}

		protected override ConfigurationElement CreateNewElement(string elementName)
		{
			Zeus.Configuration.AuthorizationRule rule = new Zeus.Configuration.AuthorizationRule();
			string str = elementName.ToLower(CultureInfo.InvariantCulture);
			if (str != null)
			{
				if (!(str == "allow"))
				{
					if (str == "deny")
						rule.Action = Zeus.Configuration.AuthorizationRuleAction.Deny;
					return rule;
				}
				rule.Action = Zeus.Configuration.AuthorizationRuleAction.Allow;
			}
			return rule;
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			Zeus.Configuration.AuthorizationRule rule = (Zeus.Configuration.AuthorizationRule) element;
			return rule.Action.ToString();
		}

		protected override bool IsElementName(string elementname)
		{
			string str;
			bool flag = false;
			if (((str = elementname.ToLower(CultureInfo.InvariantCulture)) == null) || (!(str == "allow") && !(str == "deny")))
				return flag;
			return true;
		}

		public override ConfigurationElementCollectionType CollectionType
		{
			get { return ConfigurationElementCollectionType.BasicMapAlternate; }
		}

		protected override string ElementName
		{
			get { return string.Empty; }
		}
	}
}