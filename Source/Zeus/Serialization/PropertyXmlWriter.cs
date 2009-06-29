using System;
using System.Collections.Generic;
using System.Xml;
using Isis.ExtensionMethods;
using Zeus.ContentProperties;

namespace Zeus.Serialization
{
	public class PropertyXmlWriter : IXmlWriter
	{
		public virtual void Write(ContentItem item, XmlTextWriter writer)
		{
			using (new ElementWriter("properties", writer))
			{
				foreach (PropertyData detail in GetDetails(item))
					WriteDetail(detail, writer);
			}
		}

		protected virtual IEnumerable<PropertyData> GetDetails(ContentItem item)
		{
			return item.Details.Values;
		}

		public virtual void WriteDetail(PropertyData detail, XmlTextWriter writer)
		{
			using (ElementWriter detailElement = new ElementWriter("property", writer))
			{
				detailElement.WriteAttribute("name", detail.Name);
				detailElement.WriteAttribute("typeName", detail.Value.GetType().GetTypeAndAssemblyName());

				if (detail is ObjectProperty)
				{
					string base64representation = detail.Value.ToBase64String();
					detailElement.Write(base64representation);
				}
				else if (detail.ValueType == typeof(ContentItem))
				{
					detailElement.Write(((LinkProperty) detail).LinkedItem.ID.ToString());
				}
				else if (detail.ValueType == typeof(string))//was detail.Value a typo?
				{
					string value = ((StringProperty) detail).StringValue;

					if (!string.IsNullOrEmpty(value))
						detailElement.WriteCData(value);
				}
				else if (detail.ValueType == typeof(DateTime))
				{
					detailElement.Write(ElementWriter.ToUniversalString(((DateTimeProperty) detail).DateTimeValue));
				}
				else
				{
					detailElement.Write(detail.Value.ToString());
				}
			}
		}
	}
}