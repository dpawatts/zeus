using System.Configuration;

namespace Zeus.Configuration
{
	public class DynamicContentSection : ConfigurationSection
	{
		[ConfigurationProperty("controls")]
		public DynamicContentControlCollection Controls
		{
			get { return (DynamicContentControlCollection) base["controls"]; }
		}
	}
}
