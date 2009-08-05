using System;
using System.Collections;
using System.Configuration;
using System.Globalization;
using System.Xml;

namespace Protx.Vsp
{
	public class VspConfiguration
	{
		private string _defaultCurrency;
		private string _defaultDescription;
		private string _defaultEmail;
		private string _mode;
		private string _password;
		private readonly Hashtable _servers = new Hashtable(2);
		private int _timeout;
		private string _vendor;

		internal VspConfiguration()
		{
		}

		public static VspConfiguration GetConfig()
		{
			VspConfiguration config = null;
			try
			{
				config = ConfigurationSettings.GetConfig("protx.com/vspConfig") as VspConfiguration;
			}
			catch (Exception exception)
			{
				throw new ConfigurationException("Error reading protx.com/vspConfig section from web.config", exception);
			}
			if (config == null)
			{
				throw new ConfigurationException("Error reading protx.com/vspConfig section from web.config");
			}
			return config;
		}

		protected void GetServers(XmlNode node)
		{
			foreach (XmlNode node2 in node.ChildNodes)
			{
				string name = node2.Name;
				if (name == null)
				{
					continue;
				}
				name = string.IsInterned(name);
				if (name == "add")
				{
					_servers.Add(node2.Attributes["Name"].Value, new ServerInfo(node2.Attributes));
					continue;
				}
				if (name == "remove")
				{
					_servers.Remove(node2.Attributes["Name"].Value);
					continue;
				}
				if (name == "clear")
				{
					_servers.Clear();
				}
			}
		}

		internal void LoadValuesFromConfigurationXml(XmlNode node)
		{
			XmlAttributeCollection attributes = node.Attributes;
			XmlAttribute attribute = attributes["mode"];
			if (attribute == null)
			{
				throw new ConfigurationException("mode must be specified", node);
			}
			_mode = attribute.Value;
			attribute = attributes["vendor"];
			if (attribute == null)
			{
				throw new ConfigurationException("vendor must be specified", node);
			}
			_vendor = attribute.Value.ToLower();
			attribute = attributes["defaultDescription"];
			if (attribute != null)
			{
				_defaultDescription = attribute.Value;
			}
			attribute = attributes["defaultCurrency"];
			if (attribute != null)
			{
				_defaultCurrency = attribute.Value;
			}
			else
			{
				_defaultCurrency = "GBP";
			}
			attribute = attributes["email"];
			if (attribute != null)
			{
				_defaultEmail = attribute.Value;
			}
			else
			{
				_defaultEmail = null;
			}
			attribute = attributes["timeout"];
			if (attribute != null)
			{
				_timeout = int.Parse(attribute.Value, CultureInfo.InvariantCulture);
			}
			else
			{
				_timeout = 60;
			}
			attribute = attributes["password"];
			if (attribute != null)
			{
				_password = attribute.Value;
			}
			foreach (XmlNode node2 in node.ChildNodes)
			{
				if (node2.Name == "servers")
				{
					GetServers(node2);
				}
			}
		}

		public string DefaultCurrency
		{
			get { return _defaultCurrency; }
		}

		public string DefaultDescription
		{
			get { return _defaultDescription; }
		}

		public string DefaultEmail
		{
			get { return _defaultEmail; }
		}

		public string Mode
		{
			get { return _mode; }
		}

		public string Password
		{
			get { return _password; }
		}

		public ServerInfo ServerInfo
		{
			get
			{
				if ((_mode == null) || (_mode.Length == 0))
				{
					throw new ApplicationException("Invalid configuration. No mode set.");
				}
				ServerInfo info = _servers[_mode] as ServerInfo;
				if (info == null)
				{
					throw new VspException(string.Format(CultureInfo.InvariantCulture,
					                                     "Invalid configuration. {0} mode does not have any information.",
					                                     new object[] {_mode}));
				}
				return info;
			}
		}

		public int Timeout
		{
			get { return _timeout; }
		}

		public string Vendor
		{
			get { return _vendor; }
		}
	}
}