using System;
using System.Xml;

namespace Protx.Vsp
{
	public class ServerInfo
	{
		internal Uri _directUrl;
		internal string _formUrl;
		internal string _name;
		internal Uri _serverUrl;

		internal ServerInfo(XmlAttributeCollection attributes)
		{
			_name = attributes["Name"].Value;
			_formUrl = attributes["VspFormURL"].Value;
			_serverUrl = new Uri(attributes["VspServerURL"].Value);
			_directUrl = new Uri(attributes["VspDirectURL"].Value);
		}

		public string Name
		{
			get { return _name; }
		}

		public Uri VspDirectUrl
		{
			get { return _directUrl; }
		}

		public string VspFormUrl
		{
			get { return _formUrl; }
		}

		public Uri VspServerUrl
		{
			get { return _serverUrl; }
		}
	}
}