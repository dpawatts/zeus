using System;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Zeus.BaseLibrary.Web
{
	/// <summary>
	/// Provides a secure means for transfering data within a query string.
	/// </summary>
	public class SecureQueryString : NameValueCollection
	{
		#region Fields

		/// <summary>
		/// The key used for generating the encrypted string
		/// </summary>
		private const string _CryptoKey = "IsisSecureQueryString";

		/// <summary>
		/// The Initialization Vector for the DES encryption routine
		/// </summary>
		private readonly byte[] _IV = new byte[8] { 240, 3, 45, 29, 0, 76, 173, 59 };

		#endregion

		#region Constructors

		public SecureQueryString() : base() { }

		public SecureQueryString(string encryptedString)
		{
			Deserialize(Decrypt(encryptedString));
		}

		#endregion

		#region Properties

		/// <summary>
		/// Returns the encrypted query string.
		/// </summary>
		public string EncryptedString
		{
			get { return HttpUtility.UrlEncode(Encrypt(Serialize())); }
		}

		#endregion

		#region Methods

		/// <summary>
		/// Returns the EncryptedString property.
		/// </summary>
		public override string ToString()
		{
			return EncryptedString;
		}

		/// <summary>
		/// Encrypts a serialized query string 
		/// </summary>
		private string Encrypt(string serializedQueryString)
		{
			byte[] buffer = Encoding.ASCII.GetBytes(serializedQueryString);
			TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
			MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
			des.Key = MD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(_CryptoKey));
			des.IV = _IV;
			return Convert.ToBase64String(
				des.CreateEncryptor().TransformFinalBlock(
					buffer,
					0,
					buffer.Length
					)
				);
		}

		/// <summary>
		/// Decrypts a serialized query string
		/// </summary>
		private string Decrypt(string encryptedQueryString)
		{
			try
			{
				byte[] buffer = Convert.FromBase64String(encryptedQueryString);
				TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
				MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
				des.Key = MD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(_CryptoKey));
				des.IV = _IV;
				return Encoding.ASCII.GetString(
					des.CreateDecryptor().TransformFinalBlock(
						buffer,
						0,
						buffer.Length
						)
					);
			}
			catch (CryptographicException)
			{
				throw new InvalidQueryStringException();
			}
			catch (FormatException)
			{
				throw new InvalidQueryStringException();
			}
		}

		/// <summary>
		/// Deserializes a decrypted query string and stores it
		/// as name/value pairs.
		/// </summary>
		private void Deserialize(string decryptedQueryString)
		{
			string[] nameValuePairs = decryptedQueryString.Split('&');
			for (int i = 0; i < nameValuePairs.Length; i++)
			{
				string[] nameValue = nameValuePairs[i].Split('=');
				if (nameValue.Length == 2)
				{
					base.Add(nameValue[0], nameValue[1]);
				}
			}
		}

		/// <summary>
		/// Serializes the underlying NameValueCollection as a QueryString
		/// </summary>
		private string Serialize()
		{
			StringBuilder sb = new StringBuilder();
			foreach (string key in base.AllKeys)
			{
				sb.Append(key);
				sb.Append('=');
				sb.Append(base[key]);
				sb.Append('&');
			}

			return sb.ToString();
		}

		protected string GetRequiredString(string key)
		{
			string value = this[key];

			if (value == null || value.Length == 0)
				throw new InvalidOperationException("QueryString is invalid");
			else
				return value;
		}

		protected string GetOptionalString(string key)
		{
			string value = this[key];

			if (value == null || value.Length == 0)
				return null;
			else
				return value;
		}

		public int GetRequiredInt(string key)
		{
			string value = GetRequiredString(key);
			try
			{
				return Convert.ToInt32(value);
			}
			catch (FormatException)
			{
				throw new InvalidOperationException("Requested value is not an integer");
			}
		}

		protected int? GetOptionalInt(string key)
		{
			string value = GetOptionalString(key);
			if (value != null)
			{
				try
				{
					return Convert.ToInt32(value);
				}
				catch (FormatException)
				{
					throw new InvalidOperationException("Requested value is not an integer");
				}
			}
			else
			{
				return null;
			}
		}

		#endregion
	}
}