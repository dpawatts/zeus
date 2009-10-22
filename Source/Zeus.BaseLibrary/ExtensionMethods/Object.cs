using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace Zeus.BaseLibrary.ExtensionMethods
{
	public static class ObjectExtensionMethods
	{
		/// <summary>Tries to find a property matching the supplied expression, returns null if no property is found with the first part of the expression.</summary>
		/// <param name="item">The object to query.</param>
		/// <param name="expression">The expression to evaluate.</param>
		public static object GetValue(this object item, string expression)
		{
			if (item == null) return null;

			PropertyInfo info = item.GetType().GetProperty(expression, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (info != null)
				return info.GetValue(item, new object[0]);
			if (expression.IndexOf('.') > 0)
			{
				int dotIndex = expression.IndexOf('.');
				object obj = GetValue(item, expression.Substring(0, dotIndex));
				if (obj != null)
					return GetValue(obj, expression.Substring(dotIndex + 1, expression.Length - dotIndex - 1));
			}
			return null;
		}

		public static TResult GetValueOrDefault<T, TResult>(this T parentObject, Func<T, TResult> valueCallback, TResult defaultValue)
		{
			if (parentObject != null)
				return valueCallback(parentObject);
			else
				return defaultValue;
		}

		public static void SetValue(this object parentObject, string hierarchicalPropertyName, object value, bool ignoreCase)
		{
			SetValue(parentObject, hierarchicalPropertyName, value, ".", ignoreCase);
		}

		public static void SetValue(this object parentObject, string hierarchicalPropertyName, object value, string separator, bool ignoreCase)
		{
			if (parentObject == null)
				return;

			// Get property.
			string[] propertyNames = hierarchicalPropertyName.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);
			BindingFlags propertySearchBindingFlags = ((ignoreCase) ? BindingFlags.IgnoreCase : BindingFlags.Default) | BindingFlags.Instance | BindingFlags.Public;
			object currentObject = parentObject; PropertyInfo currentProperty = null;
			for (int i = 0; i < propertyNames.Length; i++)
			{
				currentProperty = currentObject.GetType().GetProperty(propertyNames[i], propertySearchBindingFlags);
				if (currentProperty == null)
					throw new Exception(string.Format("Could not find property '{0}' on object of type '{1}'", propertyNames[i], currentObject.GetType().FullName));
				if (i < propertyNames.Length - 1)
					currentObject = currentProperty.GetValue(currentObject, null);
			}
			
			// Convert property value if necessary.
			if (value != null && value.GetType() != currentProperty.PropertyType)
			{
				value = TypeDescriptor.GetConverter(value).ConvertTo(value, currentProperty.PropertyType);
				//PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(currentObject)[currentProperty.Name];
				//value = propertyDescriptor.Converter.ConvertFrom(value);
			}

			// Set property value.
			currentProperty.SetValue(currentObject, value, null);
		}

		public static string ToBase64String(this object value)
		{
			using (MemoryStream stream = new MemoryStream())
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.Serialize(stream, value);
				return Convert.ToBase64String(stream.ToArray());
			}
		}

		public static byte[] ToSerializedBytes(this object value)
		{
			using (MemoryStream stream = new MemoryStream())
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.Serialize(stream, value);
				return stream.ToArray();
			}
		}

		public static string ToXml(this object value)
		{
			StringWriter stringWriter = new StringWriter();

			XmlSerializer xmlSerializer = new XmlSerializer(value.GetType());
			xmlSerializer.Serialize(stringWriter, value);

			string result = stringWriter.ToString();
			stringWriter.Close();

			return result;
		}
	}
}