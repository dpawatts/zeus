using System;
using System.Configuration;

namespace Zeus.Configuration
{
	public class ContentTypeSettingsElement : ConfigurationElement
	{
		[ConfigurationProperty("type", DefaultValue = "")]
		public string Type
		{
			get { return (string)base["type"]; }
			set { base["type"] = value; }
		}

		[ConfigurationProperty("zones")]
		public ContentTypeZoneCollection Zones
		{
			get { return (ContentTypeZoneCollection)base["zones"]; }
			set { base["zones"] = value; }
		}

		public bool AllSpecified
		{
			get { return Type == "*"; }
		}

		internal Type GetSystemType()
		{
			Type type = System.Type.GetType(Type);
			if (type == null)
				throw new ArgumentException("Could not load type: '" + Type + "'", "Type");
			return type;
		}
	}
}