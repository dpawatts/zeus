using System;
using System.Configuration;

namespace Zeus.Configuration
{
	public class ZeusSectionGroup : ConfigurationSectionGroup
	{
		public HostSection Host
		{
			get { return (HostSection) Sections["host"]; }
		}

		public DatabaseSection Database
		{
			get { return (DatabaseSection) Sections["database"]; }
		}
	}
}
