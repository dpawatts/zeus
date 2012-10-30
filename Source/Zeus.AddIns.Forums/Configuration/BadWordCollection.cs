using System.Configuration;

namespace Zeus.AddIns.Forums.Configuration
{
	[ConfigurationCollection(typeof(BadWordElement), CollectionType = ConfigurationElementCollectionType.BasicMap)]
	public class BadWordCollection : ConfigurationElementCollection
	{
		[ConfigurationProperty("defaultReplacement", DefaultValue = "@#$*!")]
		public string DefaultReplacement
		{
			get { return (string) base["defaultReplacement"]; }
			set { base["defaultReplacement"] = value; }
		}

		protected override ConfigurationElement CreateNewElement()
		{
			return new BadWordElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((BadWordElement) element).Word;
		}
	}
}