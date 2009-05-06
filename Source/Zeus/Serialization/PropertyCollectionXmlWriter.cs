using System.Xml;
using Zeus.ContentProperties;

namespace Zeus.Serialization
{
	public class PropertyCollectionXmlWriter : PropertyXmlWriter
	{
		public override void Write(ContentItem item, XmlTextWriter writer)
		{
			using (new ElementWriter("propertyCollections", writer))
			{
				foreach (PropertyCollection collection in item.DetailCollections.Values)
					WriteDetailCollection(writer, collection);
			}
		}

		protected virtual void WriteDetailCollection(XmlTextWriter writer, PropertyCollection collection)
		{
			using (ElementWriter collectionElement = new ElementWriter("collection", writer))
			{
				collectionElement.WriteAttribute("name", collection.Name);
				foreach (PropertyData detail in collection.Details)
					WriteDetail(detail, writer);
			}
		}
	}
}