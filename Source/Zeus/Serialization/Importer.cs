using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.XPath;
using Zeus.ContentProperties;
using Zeus.Persistence;

namespace Zeus.Serialization
{
	public class Importer
	{
		private readonly IPersister persister;
		private readonly ItemXmlReader reader;

		public Importer(IPersister persister, ItemXmlReader reader)
		{
			this.persister = persister;
			this.reader = reader;
		}

		public virtual IImportRecord Read(string path)
		{
			using (Stream input = File.OpenRead(path))
			{
				return Read(input, Path.GetFileName(path));
			}
		}

		public virtual IImportRecord Read(Stream input, string filename)
		{
			return Read(new StreamReader(input));
		}

		public virtual IImportRecord Read(TextReader input)
		{
			XPathNavigator navigator = CreateNavigator(input);

			navigator.MoveToRoot();
			if (!navigator.MoveToFirstChild())
				throw new DeserializationException("Expected root node 'zeus' not found");

			int version = ReadExportVersion(navigator);
			if (version != 1)
				throw new WrongVersionException("Invalid export version, expected 1 but was '" + version + "'");

			return reader.Read(navigator);
		}

		protected virtual XPathNavigator CreateNavigator(TextReader input)
		{
			return new XPathDocument(input).CreateNavigator();
		}

		protected virtual int ReadExportVersion(XPathNavigator navigator)
		{
			return int.Parse(navigator.GetAttribute("exportVersion", string.Empty));
		}

		public virtual void Import(IImportRecord record, ContentItem destination, ImportOptions options)
		{
			ResetIDs(record.ReadItems);
			if ((options & ImportOptions.AllItems) == ImportOptions.AllItems)
			{
				record.RootItem.AddTo(destination);
				persister.Save(record.RootItem);
			}
			else if ((options & ImportOptions.Children) == ImportOptions.Children)
			{
				RemoveReferences(record.ReadItems, record.RootItem);
				while (record.RootItem.Children.Count > 0)
				{
					ContentItem child = record.RootItem.Children[0];
					child.AddTo(destination);
					persister.Save(child);
				}
			}
			else
			{
				throw new NotImplementedException("This option isn't implemented, sorry.");
			}
		}

		protected virtual void RemoveReferences(IEnumerable<ContentItem> items, ContentItem referenceToRemove)
		{
			foreach (ContentItem item in items)
			{
				RemoveDetailReferences(referenceToRemove, item);
				RemoveReferencesInCollections(referenceToRemove, item);
			}
		}

		protected virtual void RemoveDetailReferences(ContentItem referenceToRemove, ContentItem item)
		{
			List<string> keys = new List<string>(item.Details.Keys);
			foreach (string key in keys)
			{
				PropertyData detail = item.Details[key];
				if (detail.ValueType == typeof(ContentItem))
					if (((LinkProperty) detail).LinkedItem == referenceToRemove)
						item.Details.Remove(key);
			}
		}

		protected virtual void RemoveReferencesInCollections(ContentItem referenceToRemove, ContentItem item)
		{
			foreach (PropertyCollection collection in item.DetailCollections.Values)
				for (int i = collection.Details.Count - 1; i >= 0; --i)
				{
					PropertyData detail = collection.Details[i];
					if (detail.ValueType == typeof(ContentItem))
						if (((LinkProperty) detail).LinkedItem == referenceToRemove)
							collection.Remove(referenceToRemove);
				}
		}

		protected virtual void ResetIDs(IEnumerable<ContentItem> items)
		{
			foreach (ContentItem item in items)
				item.ID = 0;
		}
	}
}