using System;
using System.Collections.Generic;
using System.Xml.XPath;
using Zeus.BaseLibrary.ExtensionMethods;
using Zeus.ContentProperties;

namespace Zeus.Serialization
{
	public class PropertyCollectionXmlReader : XmlReader, IXmlReader
	{
		public void Read(XPathNavigator navigator, ContentItem item, ReadingJournal journal)
		{
			foreach (XPathNavigator detailCollectionElement in EnumerateChildren(navigator))
				ReadDetailCollection(detailCollectionElement, item, journal);
		}

		protected void ReadDetailCollection(XPathNavigator navigator, ContentItem item, ReadingJournal journal)
		{
			Dictionary<string, string> attributes = GetAttributes(navigator);
			string name = attributes["name"];

			foreach (XPathNavigator detailElement in EnumerateChildren(navigator))
				ReadDetail(detailElement, item.GetDetailCollection(name, true), journal);
		}

		protected virtual void ReadDetail(XPathNavigator navigator, PropertyCollection collection, ReadingJournal journal)
		{
			Dictionary<string, string> attributes = GetAttributes(navigator);
			Type type = attributes["typeName"].ToType();

			if (type != typeof(ContentItem))
			{
				collection.Add(Parse(navigator.Value, type));
			}
			else
			{
				int referencedItemID = int.Parse(navigator.Value);
				ContentItem referencedItem = journal.Find(referencedItemID);
				if (referencedItem != null)
					collection.Add(referencedItem);
				else
					journal.ItemAdded += (sender, e) =>
        	{
        		if (e.AffectedItem.ID == referencedItemID)
        		{
        			collection.Add(e.AffectedItem);
        		}
        	};
			}
		}
	}
}