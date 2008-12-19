using System;
using System.Configuration;

namespace Zeus.Configuration
{
	public class ZeusSectionGroup : ConfigurationSectionGroup
	{
		public AdminSection Admin
		{
			get { return (AdminSection) Sections["admin"]; }
		}

		public HostSection Host
		{
			get { return (HostSection) Sections["host"]; }
		}

		public DatabaseSection Database
		{
			get { return (DatabaseSection) Sections["database"]; }
		}

		public FileSystemSection FileSystem
		{
			get { return (FileSystemSection) Sections["fileSystem"]; }
		}

		public MembershipSection Membership
		{
			get { return (MembershipSection) Sections["membership"]; }
		}
	}
}
