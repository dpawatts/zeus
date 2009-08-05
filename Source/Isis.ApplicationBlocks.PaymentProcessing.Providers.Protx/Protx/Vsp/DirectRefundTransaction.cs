using System;

namespace Protx.Vsp
{
	public class DirectRefundTransaction : VspCardTransaction
	{
		public DirectRefundTransaction() : this(Guid.NewGuid().ToString())
		{
		}

		public DirectRefundTransaction(string vendorTxCode)
			: base("2.22", VspServiceType.DirectRefundTx, VspTransactionType.DirectRefund, vendorTxCode)
		{
		}

		public DirectRefundResponse Send()
		{
			return new DirectRefundResponse(this, base.InternalSend());
		}
	}
}