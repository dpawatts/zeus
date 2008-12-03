using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Resources;
using System.Data.Objects.DataClasses;

namespace Zeus
{
	/// <summary>
	/// Mixed utility functions used by Zeus.
	/// </summary>
	public static class Utility
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
				else if (destinationType.IsEnum && value is int)
					return Enum.ToObject(destinationType, (int) value);
				else if (!destinationType.IsAssignableFrom(value.GetType()))
					return System.Convert.ChangeType(value, destinationType);
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

		/// <summary>Tries to find a property matching the supplied expression, returns null if no property is found with the first part of the expression.</summary>
		/// <param name="item">The object to query.</param>
		/// <param name="expression">The expression to evaluate.</param>
		public static object Evaluate(object item, string expression)
		{
			if (item == null) return null;

			PropertyInfo info = item.GetType().GetProperty(expression);
			if (info != null)
				return info.GetValue(item, new object[0]);
			else if (expression.IndexOf('.') > 0)
			{
				int dotIndex = expression.IndexOf('.');
				object obj = Evaluate(item, expression.Substring(0, dotIndex));
				if (obj != null)
					return Evaluate(obj, expression.Substring(dotIndex + 1, expression.Length - dotIndex - 1));
			}
			return null;
		}

		/// <summary>Evaluates an expression and applies a format string.</summary>
		/// <param name="item">The object to query.</param>
		/// <param name="expression">The expression to evaluate.</param>
		/// <param name="format">The format string to apply.</param>
		/// <returns>The formatted result ov the evaluation.</returns>
		public static string Evaluate(object item, string expression, string format)
		{
			return string.Format(format, Evaluate(item, expression));
		}
	}
}
