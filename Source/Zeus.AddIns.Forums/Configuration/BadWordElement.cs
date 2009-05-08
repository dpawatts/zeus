using System.Configuration;

namespace Zeus.AddIns.Forums.Configuration
{
	public class BadWordElement : ConfigurationElement
	{
		[ConfigurationProperty("word", IsRequired = true, IsKey = true, DefaultValue = "")]
		public string Word
		{
			get { return (string) base["word"]; }
			set { base["word"] = value; }
		}

		[ConfigurationProperty("replacement")]
		public string Replacement
		{
			get { return (string) base["replacement"]; }
			set { base["replacement"] = value; }
		}
	}
}