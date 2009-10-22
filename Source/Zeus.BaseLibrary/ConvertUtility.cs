using System;
using System.ComponentModel;

namespace Zeus.BaseLibrary
{
	public static class ConvertUtility
	{
		/// <summary>Converts a value to a destination type.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="destinationType">The type to convert the value to.</param>
		/// <returns>The converted value.</returns>
		public static object Convert(object value, Type destinationType)
		{
			if (value != null)
			{
				TypeConverter converter = TypeDescriptor.GetConverter(destinationType);
				if (converter != null && converter.CanConvertFrom(value.GetType()))
					return converter.ConvertFrom(value);
				converter = TypeDescriptor.GetConverter(value.GetType());
				if (converter != null && converter.CanConvertTo(destinationType))
					return converter.ConvertTo(value, destinationType);
				if (destinationType.IsEnum && value is int)
					return Enum.ToObject(destinationType, (int) value);
				if (!destinationType.IsAssignableFrom(value.GetType()))
				{
					if (!(value is IConvertible))
						throw new Exception(string.Format("Cannot convert object of type '{0}' because it does not implement IConvertible", value.GetType()));
					return System.Convert.ChangeType(value, destinationType);
				}
			}
			return value;
		}

		/// <summary>Converts a value to a destination type.</summary>
		/// <param name="value">The value to convert.</param>
		/// <typeparam name="T">The type to convert the value to.</typeparam>
		/// <returns>The converted value.</returns>
		public static T Convert<T>(object value)
		{
			return (T) Convert(value, typeof(T));
		}
	}
}