using System.Configuration;
using System.Xml;

namespace Protx.Vsp
{
	internal class VspConfigurationHandler : IConfigurationSectionHandler
	{
		public object Create(object parent, object configContext, XmlNode section)
		{
			VspConfiguration configuration = new VspConfiguration();
			configuration.LoadValuesFromConfigurationXml(section);
			return configuration;
		}
	}
}