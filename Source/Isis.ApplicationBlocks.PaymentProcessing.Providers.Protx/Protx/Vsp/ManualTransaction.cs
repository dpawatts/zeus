using System;

namespace Protx.Vsp
{
	public class ManualTransaction : VspCardDetailedTransaction
	{
		public ManualTransaction() : this(Guid.NewGuid().ToString())
		{
		}

		public ManualTransaction(string vendorTxCode)
			: base("2.22", VspServiceType.VendorManualTx, VspTransactionType.Manual, vendorTxCode)
		{
		}

		public ManualResponse Send()
		{
			return new ManualResponse(this, base.InternalSend());
		}

		public string AuthCode
		{
			get { return base.Parameters.AuthCode; }
			set { base.Parameters.AuthCode = value; }
		}
	}
}