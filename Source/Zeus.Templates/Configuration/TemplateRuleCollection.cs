using System.Configuration;
using System.Globalization;
using Zeus.ContentTypes;

namespace Zeus.Templates.Configuration
{
	[ConfigurationCollection(typeof(TemplateRule), AddItemName = "allow,deny", CollectionType = ConfigurationElementCollectionType.BasicMapAlternate)]
	public class TemplateRuleCollection : ConfigurationElementCollection
	{
		public override ConfigurationElementCollectionType CollectionType
		{
			get { return ConfigurationElementCollectionType.BasicMapAlternate; }
		}

		protected override string ElementName
		{
			get { return string.Empty; }
		}

		protected override ConfigurationElement CreateNewElement()
		{
			return new TemplateRule();
		}

		protected override ConfigurationElement CreateNewElement(string elementName)
		{
			TemplateRule rule = new TemplateRule();
			string str = elementName.ToLower(CultureInfo.InvariantCulture);
			if (str != null)
			{
				if (str != "allow")
				{
					if (str == "deny")
						rule.Action = TemplateRuleAction.Deny;
					return rule;
				}
				rule.Action = TemplateRuleAction.Allow;
			}
			return rule;
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			TemplateRule rule = (TemplateRule) element;
			return rule.Action.ToString();
		}

		protected override bool IsElementName(string elementName)
		{
			string str = elementName.ToLower(CultureInfo.InvariantCulture);
			return (str == "allow" || str == "deny");
		}

		internal bool IsContentTypeAllowed(ContentType contentType)
		{
			if (Count == 0)
				return true;

			if (contentType != null)
				foreach (TemplateRule rule in this)
				{
					int num = rule.IsContentTypeAllowed(contentType);
					if (num != 0)
						return (num > 0);
				}
			return false;
		}
	}
}