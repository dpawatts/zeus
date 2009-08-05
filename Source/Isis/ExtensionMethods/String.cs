using System;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Isis.ExtensionMethods
{
	public static class StringExtensionMethods
	{
		public static bool Contains(this string thisString, string value, StringComparison comparisonType)
		{
			return (thisString.IndexOf(value, comparisonType) != -1);
		}

		public static T Deserialize<T>(this string serializedValue)
		{
			return (T) Deserialize(serializedValue, typeof(T));
		}

		public static object Deserialize(this string serializedValue, Type type)
		{
			byte[] serialisedBytes = Convert.FromBase64String(serializedValue);
			using (MemoryStream stream = new MemoryStream(serialisedBytes))
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				return binaryFormatter.Deserialize(stream);
			}
		}

		public static string FromBase64String(this string value)
		{
			ASCIIEncoding encoding = new ASCIIEncoding();
			Decoder decoder = encoding.GetDecoder();
			byte[] toDecodeBytes = Convert.FromBase64String(value);
			int charCount = decoder.GetCharCount(toDecodeBytes, 0, toDecodeBytes.Length);
			char[] decodedChars = new char[charCount];
			decoder.GetChars(toDecodeBytes, 0, toDecodeBytes.Length, decodedChars, 0);
			string result = new string(decodedChars);
			return result;
		}

		public static T FromXml<T>(this string xml)
		{
			StringReader stringReader = new StringReader(xml);

			XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
			T result = (T) xmlSerializer.Deserialize(stringReader);

			stringReader.Close();

			return result;
		}

		public static string Left(this string value, int length)
		{
			return (length >= value.Length) ? value : value.Substring(0, length);
		}

		public static string Paragraph(this string value, int paragraphIndex)
		{
			string[] paragraphs = value.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
			return paragraphs.Length > paragraphIndex ? paragraphs[paragraphIndex] : string.Empty;
		}

		public static string Paragraphs(this string value, int paragraphIndex)
		{
			string[] paragraphs = value.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
			if (paragraphs.Length > paragraphIndex)
				return string.Join(Environment.NewLine, paragraphs, paragraphIndex, paragraphs.Length - paragraphIndex);
			else
				return string.Empty;
		}

		public static string Right(this string value, int length)
		{
			return (length >= value.Length) ? value : value.Substring(value.Length - length, length);
		}

		public static string RemoveHtmlTags(this string value)
		{
			const string expression = "<.*?>";
			return Regex.Replace(value, expression, string.Empty);
		}

        public static string RemoveWrappingHtmlTags(this string value)
        {
            return Regex.Replace(value, @"^<.*?>(.*)<.*?>?", "$1");
        }

		public static string ToBase64String(this string value)
		{
			byte[] encodedDataBytes = Encoding.ASCII.GetBytes(value);
			string encodedData = Convert.ToBase64String(encodedDataBytes);
			return encodedData;
		}

		public static string ToPascalCase(this string value)
		{
			if (string.IsNullOrEmpty(value))
				return value;

			return char.ToLower(value[0]) + value.Substring(1);
		}

		public static string ToSafeUrl(this string value)
		{
			if (string.IsNullOrEmpty(value))
				return string.Empty;

			string result = value.Replace(' ', '-').ToLower();
			result = Regex.Replace(result, "[^a-zA-Z0-9\\-]", string.Empty);
			return result;
		}

		public static T ToEnum<T>(this string value)
		{
			return (T) Enum.Parse(typeof(T), value);
		}

		/// <summary>Gets the type from a string.</summary>
		/// <param name="typeName">The type name string.</param>
		/// <returns>The type.</returns>
		public static Type ToType(this string typeName)
		{
			if (typeName == null)
				throw new ArgumentNullException("name");

			Type t = Type.GetType(typeName);
			if (t == null)
				throw new IsisException("Couldn't find any type with the name '{0}'", typeName);

			return t;
		}

		public static string Truncate(this string value, int length)
		{
			return Truncate(value, length, true);
		}

		public static string Truncate(this string value, int length, bool preserveWordBoundaries)
		{
			if (string.IsNullOrEmpty(value))
				return value;

			if (value.Length <= length)
				return value;

			string[] words = value.Split(' ');
			int currentLength = 0;
			StringBuilder sb = new StringBuilder();
			for (int i = 0, arrayLength = words.Length; i < arrayLength && currentLength < length - 3; ++i)
			{
				string newEntry = words[i] + " ";
				sb.Append(newEntry);
				currentLength += newEntry.Length;
			}

			return sb.ToString() + "...";
		}
	}
}