using System.Xml.XPath;

namespace Zeus.Serialization
{
	public interface IXmlReader
	{
		void Read(XPathNavigator navigator, ContentItem item, ReadingJournal journal);
	}
}