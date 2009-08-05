using System;

namespace Protx.Vsp
{
	public class NotificationResponseEventArgs : EventArgs
	{
		private readonly VspStatusType _status;
		private readonly string _vendorTxCode;

		internal NotificationResponseEventArgs(string vendorTxCode, VspStatusType status)
		{
			_vendorTxCode = vendorTxCode;
			_status = status;
		}

		public bool Handled { get; set; }

		public VspStatusType Status
		{
			get { return _status; }
		}

		public string VendorTxCode
		{
			get { return _vendorTxCode; }
		}
	}
}