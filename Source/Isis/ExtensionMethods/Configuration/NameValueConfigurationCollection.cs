using System.Collections.Specialized;
using System.Configuration;

namespace Isis.ExtensionMethods.Configuration
{
	public static class NameValueConfigurationCollectionExtensionMethods
	{
		public static NameValueCollection ToNameValueCollection(this NameValueConfigurationCollection collection)
		{
			NameValueCollection result = new NameValueCollection();
			foreach (string key in collection)
				result.Add(key, collection[key].Value);
			return result;
		}
	}
}
