using System.Configuration;

namespace Isis.ApplicationBlocks.DataMigrations.Configuration
{
	public class MigrationsSection : ConfigurationSection
	{
		[ConfigurationProperty("connectionStringName")]
		public string ConnectionStringName
		{
			get { return this["connectionStringName"] as string; }
			set { this["connectionStringName"] = value; }
		}
	}
}