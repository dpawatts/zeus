using System;

namespace Protx.Vsp
{
	public class NotificationEventArgs : EventArgs
	{
		private readonly string _vendorTxCode;

		internal NotificationEventArgs(string vendorTxCode)
		{
			_vendorTxCode = vendorTxCode;
		}

		public string VendorTxCode
		{
			get { return _vendorTxCode; }
		}
	}
}