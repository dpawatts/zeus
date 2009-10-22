using System;
using System.Web;
using Zeus.BaseLibrary.Web;

namespace Zeus.BaseLibrary.ExtensionMethods.Web
{
	public static class HttpRequestExtensionMethods
	{
		public static string GetOptionalString(this HttpRequest request, string key)
		{
			string value = request[key];

			if (string.IsNullOrEmpty(value))
				return null;
			else
				return value;
		}

		public static string GetRequiredString(this HttpRequest request, string key)
		{
			string value = GetOptionalString(request, key);
			if (value == null)
				throw new InvalidOperationException("QueryString is invalid");
			else
				return value;
		}

		public static int? GetOptionalInt(this HttpRequest request, string key)
		{
			string value = GetOptionalString(request, key);
			if (value != null)
			{
				int result;
				if (int.TryParse(value, out result))
					return result;
				else
					throw new InvalidOperationException("Requested value is not an integer");
			}
			else
			{
				return null;
			}
		}

		public static T? GetOptionalEnum<T>(this HttpRequest request, string key)
			where T : struct
		{
			string value = GetOptionalString(request, key);
			if (value != null)
			{
				try
				{
					T result = (T) Enum.Parse(typeof(T), value, true);
					return result;
				}
				catch (ArgumentException)
				{
					throw new InvalidOperationException("Requested value is not a valid enum value");
				}
			}
			else
			{
				return null;
			}
		}

		public static T GetRequiredEnum<T>(this HttpRequest request, string key)
			where T : struct
		{
			T? value = GetOptionalEnum<T>(request, key);
			if (value == null)
				throw new InvalidOperationException("QueryString is invalid");
			else
				return value.Value;
		}

		public static int GetRequiredInt(this HttpRequest request, string key)
		{
			int? value = GetOptionalInt(request, key);
			if (value == null)
				throw new InvalidOperationException("QueryString is invalid");
			else
				return value.Value;
		}

		public static bool? GetOptionalBool(this HttpRequest request, string key)
		{
			string value = GetOptionalString(request, key);
			if (value != null)
			{
				bool result;
				if (bool.TryParse(value, out result))
					return result;
				else
					throw new InvalidOperationException("Requested value is not a boolean");
			}
			else
			{
				return null;
			}
		}

		public static bool GetRequiredBool(this HttpRequest request, string key)
		{
			bool? value = GetOptionalBool(request, key);
			if (value == null)
				throw new InvalidOperationException("QueryString is invalid");
			else
				return value.Value;
		}

		public static SecureQueryString GetSecureQueryString(this HttpRequest request)
		{
			return new SecureQueryString(request.QueryString["x"]);
		}
	}
}