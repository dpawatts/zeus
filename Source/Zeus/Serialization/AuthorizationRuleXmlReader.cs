using System.Collections.Generic;
using System.Xml.XPath;
using Zeus.Security;

namespace Zeus.Serialization
{
	public class AuthorizationRuleXmlReader : XmlReader, IXmlReader
	{
		public void Read(XPathNavigator navigator, ContentItem item, ReadingJournal journal)
		{
			foreach (XPathNavigator authorizationElement in EnumerateChildren(navigator))
			{
				Dictionary<string, string> attributes = GetAttributes(authorizationElement);
				string operation = attributes["operation"];
				string role = attributes["role"];
				string user = attributes["user"];
				item.AuthorizationRules.Add(new AuthorizationRule(item, operation, role, user));
			}
		}
	}
}