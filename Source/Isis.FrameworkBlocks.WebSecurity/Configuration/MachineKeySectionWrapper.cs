using System;
using System.Web.Configuration;
using System.Configuration;
using System.Reflection;

namespace Isis.Web.Configuration
{
	/// <summary>
	/// Wrapper for internal methods on System.Web.Configuration.MachineKeySection
	/// </summary>
	public static class MachineKeySectionWrapper
	{
		private static MethodInfo _hashData, _encOrDecData, _hexStringToByteArray, _byteArrayToHexString;

		static MachineKeySectionWrapper()
		{
			MachineKeySection config = (MachineKeySection) ConfigurationManager.GetSection("system.web/machineKey");
			Type machineKeyType = config.GetType();

			BindingFlags bf = BindingFlags.NonPublic | BindingFlags.Static;

			Type[] typeArray = new Type[5];
			typeArray.SetValue(typeof(Boolean), 0);
			typeArray.SetValue(typeof(Byte[]), 1);
			typeArray.SetValue(typeof(Byte[]), 2);
			typeArray.SetValue(typeof(Int32), 3);
			typeArray.SetValue(typeof(Int32), 4);

			_hashData = machineKeyType.GetMethod("HashData", bf, null,
				new Type[]
				{
					typeof(byte[]),
					typeof(byte[]),
					typeof(int),
					typeof(int)
				}, null);

			_encOrDecData = machineKeyType.GetMethod("EncryptOrDecryptData", bf, null, typeArray, null);
			_hexStringToByteArray = machineKeyType.GetMethod("HexStringToByteArray", bf);
			_byteArrayToHexString = machineKeyType.GetMethod("ByteArrayToHexString", bf);

			if (_encOrDecData == null || _hexStringToByteArray == null || _byteArrayToHexString == null)
				throw new InvalidOperationException("Unable to get the methods to invoke.");
		}

		public static byte[] HashData(byte[] buf, byte[] modifier, int start, int length)
		{
			return (byte[]) _hashData.Invoke(null, new object[] { buf, modifier, start, length });
		}

		public static byte[] EncryptOrDecryptData(bool fEncrypt, byte[] buf, byte[] modifier, int start, int length)
		{
			return (byte[]) _encOrDecData.Invoke(null, new object[] { fEncrypt, buf, modifier, start, length });
		}

		public static string ByteArrayToHexString(byte[] buf, int iLen)
		{
			return (string) _byteArrayToHexString.Invoke(null, new object[] { buf, iLen });
		}

		public static byte[] HexStringToByteArray(string str)
		{
			return (byte[]) _hexStringToByteArray.Invoke(null, new object[] { str });
		}
	}
}
