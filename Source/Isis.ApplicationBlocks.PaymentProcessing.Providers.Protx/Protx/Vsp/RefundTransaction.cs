using System;

namespace Protx.Vsp
{
	public class RefundTransaction : VspRelatedTransaction
	{
		public RefundTransaction() : this(Guid.NewGuid().ToString())
		{
		}

		public RefundTransaction(VspSecuredResponse originalResponse) : this(Guid.NewGuid().ToString(), originalResponse)
		{
		}

		public RefundTransaction(string vendorTxCode)
			: base("2.22", VspServiceType.VendorRefundTx, VspTransactionType.Refund, vendorTxCode)
		{
		}

		public RefundTransaction(string vendorTxCode, VspSecuredResponse originalResponse)
			: base("2.22", VspServiceType.VendorRefundTx, VspTransactionType.Refund, vendorTxCode, originalResponse)
		{
		}

		public RefundResponse Send()
		{
			return new RefundResponse(this, base.InternalSend());
		}
	}
}