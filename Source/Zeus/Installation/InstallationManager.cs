using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using SoundInTheory.NMigration;
using System.Reflection;

namespace Zeus.Installation
{
	public class InstallationManager
	{
		public static void Install()
		{
			string connectionString = ConfigurationManager.ConnectionStrings["zeus"].ConnectionString;
			MigrationManager.Migrate(connectionString, Assembly.GetExecutingAssembly(), "Zeus.Installation.Migrations");
			//MigrationManager.Migrate(connectionString, Assembly.GetExecutingAssembly(), "Zeus.FileSystem.Migrations");
		}
	}
}
