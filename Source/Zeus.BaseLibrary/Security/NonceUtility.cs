using System;
using System.Security.Cryptography;

namespace Zeus.BaseLibrary.Security
{
	public static class NonceUtility
	{
		public static string GenerateNonce()
		{
			RNGCryptoServiceProvider cryptoProvider = new RNGCryptoServiceProvider();
			byte[] data = new byte[32];
			cryptoProvider.GetBytes(data);
			return Convert.ToBase64String(data);
		}
	}
}