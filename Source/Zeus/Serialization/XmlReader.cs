using System;
using System.Collections.Generic;
using System.Xml.XPath;
using Zeus.BaseLibrary.ExtensionMethods;
using Zeus.ContentProperties;

namespace Zeus.Serialization
{
	public abstract class XmlReader
	{
		public static Dictionary<string, string> GetAttributes(XPathNavigator navigator)
		{
			if (!navigator.MoveToFirstAttribute())
				throw new DeserializationException("Node has no attributes: " + navigator.Name);

			Dictionary<string, string> attributes = new Dictionary<string, string>();
			do
			{
				attributes.Add(navigator.Name, navigator.Value);
			} while (navigator.MoveToNextAttribute());

			navigator.MoveToParent();

			return attributes;
		}

		public static object Parse(string value, Type type)
		{
			if (Context.Current.Resolve<IContentPropertyManager>().CreatePropertyDataObject(type) is ObjectProperty)
				return value.Deserialize(type);
			
			if (type == typeof(DateTime))
				return ToNullableDateTime(value);

			return Utility.Convert(value, type);
		}

		public static IEnumerable<XPathNavigator> EnumerateChildren(XPathNavigator navigator)
		{
			if (navigator.MoveToFirstChild())
			{
				do
				{
					yield return navigator;
				} while (navigator.MoveToNext());

				navigator.MoveToParent();
			}
		}

		public static DateTime? ToNullableDateTime(string value)
		{
			if (string.IsNullOrEmpty(value))
				return null;

			return DateTime.Parse(value, System.Globalization.CultureInfo.InvariantCulture).ToLocalTime();
		}
	}
}