using System.Collections.Generic;
using System.Xml.XPath;
using Zeus.Globalization;
using Zeus.Security;

namespace Zeus.Serialization
{
	public class LanguageSettingXmlReader : XmlReader, IXmlReader
	{
		public void Read(XPathNavigator navigator, ContentItem item, ReadingJournal journal)
		{
			foreach (XPathNavigator languageSettingElement in EnumerateChildren(navigator))
			{
				Dictionary<string, string> attributes = GetAttributes(languageSettingElement);
				string language = attributes["language"];
				string fallbackLanguage = attributes["fallbackLanguage"];
				item.LanguageSettings.Add(new LanguageSetting(item, language, fallbackLanguage));
			}
		}
	}
}