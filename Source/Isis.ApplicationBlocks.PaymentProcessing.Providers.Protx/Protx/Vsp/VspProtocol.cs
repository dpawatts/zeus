using System;
using System.Text;
using System.Web.Security;

namespace Protx.Vsp
{
	public class VspProtocol
	{
		public const string DirectProtocolVersion = "2.22";
		public const string FormProtocolVersion = "2.22";
		public const string ServerProtocolVersion = "2.22";

		private VspProtocol()
		{
		}

		public static string CalculateMd5Hash(string plainText)
		{
			return FormsAuthentication.HashPasswordForStoringInConfigFile(plainText, "MD5");
		}

		public static AvsCv2Result ConvertToAvsCv2(string value)
		{
			switch (value)
			{
				case "NO DATA MATCHES":
					return AvsCv2Result.None;

				case "SECURITY CODE MATCH ONLY":
					return AvsCv2Result.SecurityCode;

				case "ADDRESS MATCH ONLY":
					return AvsCv2Result.Address;

				case "ALL MATCH":
					return AvsCv2Result.All;

				case "DATA NOT CHECKED":
					return AvsCv2Result.Skipped;
			}
			throw new VspException("Invalid AVSCV2 value");
		}

		internal static string FormCrypt(VspTransactionParametersCollection parameters)
		{
			string password = VspConfiguration.GetConfig().Password;
			if ((password == null) || (password.Length == 0))
			{
				throw new VspException("Configuration error: Password not set for VSP Form request");
			}
			byte[] bytes = Encoding.ASCII.GetBytes(password);
			int length = bytes.Length;
			StringBuilder builder = new StringBuilder(200);
			bool flag = true;
			foreach (string str2 in parameters.AllKeys)
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					builder.Append('&');
				}
				builder.Append(str2);
				builder.Append('=');
				builder.Append(parameters[str2]);
			}
			byte[] buffer2 = Encoding.GetEncoding(0x4e4).GetBytes(builder.ToString());
			int num2 = buffer2.Length;
			byte[] inArray = new byte[num2];
			int index = 0;
			for (int i = 0; i < num2; i++)
			{
				inArray[i] = (byte) (buffer2[i] ^ bytes[index]);
				if (++index >= length)
				{
					index = 0;
				}
			}
			return Convert.ToBase64String(inArray);
		}

		public static string FormDecrypt(string encrypted)
		{
			byte[] buffer = Convert.FromBase64String(encrypted);
			int length = buffer.Length;
			char[] chArray = new char[length];
			string password = VspConfiguration.GetConfig().Password;
			if ((password == null) || (password.Length == 0))
			{
				throw new VspException("Configuration error: Password not set for VSP Form request");
			}
			byte[] bytes = Encoding.UTF8.GetBytes(password);
			int num2 = bytes.Length;
			int index = 0;
			for (int i = 0; i < length; i++)
			{
				chArray[i] = (char) (buffer[i] ^ bytes[index]);
				if (++index >= num2)
				{
					index = 0;
				}
			}
			return new string(chArray);
		}
	}
}