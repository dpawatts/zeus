using System.Configuration;
using System.Globalization;

namespace Isis.Web.Configuration
{
	[ConfigurationCollection(typeof(AuthorizationRule), AddItemName = "allow,deny", CollectionType = ConfigurationElementCollectionType.BasicMapAlternate)]
	public sealed class AuthorizationRuleCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new AuthorizationRule();
		}

		protected override ConfigurationElement CreateNewElement(string elementName)
		{
			AuthorizationRule rule = new AuthorizationRule();
			string str = elementName.ToLower(CultureInfo.InvariantCulture);
			if (str != null)
			{
				if (!(str == "allow"))
				{
					if (str == "deny")
						rule.Action = AuthorizationRuleAction.Deny;
					return rule;
				}
				rule.Action = AuthorizationRuleAction.Allow;
			}
			return rule;
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			AuthorizationRule rule = (AuthorizationRule) element;
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