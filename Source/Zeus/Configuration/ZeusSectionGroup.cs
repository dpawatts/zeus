using System.Configuration;

namespace Zeus.Configuration
{
	public class ZeusSectionGroup : ConfigurationSectionGroup
	{
		public AdminSection Admin
		{
			get { return (AdminSection) Sections["admin"]; }
		}

		public DynamicContentSection DynamicContent
		{
			get { return (DynamicContentSection) Sections["dynamicContent"]; }
		}

		public GlobalizationSection Globalization
		{
			get { return (GlobalizationSection) Sections["globalization"]; }
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

		public AuthenticationSection Authentication
		{
			get { return (AuthenticationSection)Sections["authentication"]; }
		}

		public AuthorizationSection Authorization
		{
			get { return (AuthorizationSection)Sections["authorization"]; }
		}
	}
}
