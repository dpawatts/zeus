using System;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace Isis.Security
{
	public static class Impersonation
	{
		[DllImport("advapi32.dll", SetLastError = true)]
		private static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		private static extern bool CloseHandle(IntPtr handle);
 
		public static WindowsIdentity CreateIdentity(string user, string domain, string password)
		{
			IntPtr phToken = IntPtr.Zero;
			if (!LogonUser(user, domain, password, 3, 0, ref phToken))
			{
				int num = Marshal.GetLastWin32Error();
				throw new Exception("LogonUser failed with error code: " + num);
			}
			WindowsIdentity identity = new WindowsIdentity(phToken);
			CloseHandle(phToken);
			return identity;
		}

		public static void RunImpersonatedCode(string user, string domain, string password, Action callback)
		{
			using (WindowsIdentity wi = CreateIdentity(user, domain, password))
			{
				using (WindowsImpersonationContext wic = wi.Impersonate())
				{
					try
					{
						callback();
					}
					finally
					{
						wic.Undo();
					}
				}
			}
		}
	}
}
