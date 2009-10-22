using System;
using System.Collections.Generic;
using System.Xml.XPath;
using Zeus.BaseLibrary.ExtensionMethods;

namespace Zeus.Serialization
{
	/// <summary>
	/// Reads a content detail from the input navigator.
	/// </summary>
	public class PropertyXmlReader : XmlReader, IXmlReader
	{
		public void Read(XPathNavigator navigator, ContentItem item, ReadingJournal journal)
		{
			foreach (XPathNavigator detailElement in EnumerateChildren(navigator))
				ReadDetail(detailElement, item, journal);
		}

		protected virtual void ReadDetail(XPathNavigator navigator, ContentItem item, ReadingJournal journal)
		{
			Dictionary<string, string> attributes = GetAttributes(navigator);
			Type type = attributes["typeName"].ToType();

			string name = attributes["name"];

			if (type != typeof(ContentItem))
			{
				item[name] = Parse(navigator.Value, type);
			}
			else
			{
				int referencedItemID = int.Parse(navigator.Value);
				ContentItem referencedItem = journal.Find(referencedItemID);
				if (referencedItem != null)
					item[name] = referencedItem;
				else
					journal.ItemAdded += (sender, e) =>
        	{
        		if (e.AffectedItem.ID == referencedItemID)
        			item[name] = e.AffectedItem;
        	};
			}
		}
	}
}