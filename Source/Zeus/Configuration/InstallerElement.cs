using System.Configuration;

namespace Zeus.Configuration
{
	public class InstallerElement : ConfigurationElement
	{
		/// <summary>When set to true this setting will cause the database connection to be verified upon startup. If the database connection is down the first user is redirected to an installation screen.</summary>
		[ConfigurationProperty("checkInstallationStatus", DefaultValue = false)]
		public bool CheckInstallationStatus
		{
			get { return (bool) base["checkInstallationStatus"]; }
			set { base["checkInstallationStatus"] = value; }
		}

		[ConfigurationProperty("mode", DefaultValue = InstallationMode.Normal)]
		public InstallationMode Mode
		{
			get { return (InstallationMode) base["mode"]; }
			set { base["mode"] = value; }
		}
	}
}