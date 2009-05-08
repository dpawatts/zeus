using System.Configuration;

namespace Zeus.AddIns.Forums.Configuration
{
	public class ForumSection : ConfigurationSection
	{
		[ConfigurationProperty("badWords")]
		public BadWordCollection BadWords
		{
			get { return (BadWordCollection) base["badWords"]; }
		}
	}
}