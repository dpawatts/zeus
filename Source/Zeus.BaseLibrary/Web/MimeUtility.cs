using System;
using System.Runtime.InteropServices;

namespace Isis.Web
{
	public static class MimeUtility
	{
		[DllImport("urlmon.dll", CharSet = CharSet.Auto)]
		private extern static UInt32 FindMimeFromData(
				UInt32 pBC,
				[MarshalAs(UnmanagedType.LPStr)] string pwzUrl,
				[MarshalAs(UnmanagedType.LPArray)] byte[] pBuffer,
				UInt32 cbSize,
				[MarshalAs(UnmanagedType.LPStr)] string pwzMimeProposed,
				UInt32 dwMimeFlags,
				out UInt32 ppwzMimeOut,
				UInt32 dwReserverd
		);

		public static string GetMimeType(byte[] fileData)
		{
			// We only care about the first 256 bytes.
			byte[] buffer = new byte[256];
			if (fileData.Length >= 256)
				Buffer.BlockCopy(fileData, 0, buffer, 0, 256);
			else
				Buffer.BlockCopy(fileData, 0, buffer, 0, fileData.Length);

			try
			{
				UInt32 mimetype;
				FindMimeFromData(0, null, buffer, 256, null, 0, out mimetype, 0);
				IntPtr mimeTypePtr = new IntPtr(mimetype);
				string mime = Marshal.PtrToStringUni(mimeTypePtr);
				Marshal.FreeCoTaskMem(mimeTypePtr);
				return mime;
			}
			catch
			{
				return "application/octet-stream";
			}
		}
	}
}