using System;

namespace Protx.Vsp
{
	public class RepeatTransaction : VspRelatedTransaction
	{
		public RepeatTransaction() : this(Guid.NewGuid().ToString())
		{
		}

		public RepeatTransaction(VspSecuredResponse originalResponse) : this(Guid.NewGuid().ToString(), originalResponse)
		{
		}

		public RepeatTransaction(string vendorTxCode) : this(vendorTxCode, VspTransactionType.Repeat)
		{
		}

		public RepeatTransaction(string vendorTxCode, VspSecuredResponse originalResponse)
			: this(vendorTxCode, VspTransactionType.Repeat, originalResponse)
		{
		}

		public RepeatTransaction(string vendorTxCode, VspTransactionType txType)
			: base("2.22", VspServiceType.VendorRepeatTx, txType, vendorTxCode)
		{
			ValidateTranactionType(base.TxType, new[] {VspTransactionType.Repeat, VspTransactionType.RepeatDeferred});
		}

		public RepeatTransaction(string vendorTxCode, VspTransactionType txType, VspSecuredResponse originalResponse)
			: base("2.22", VspServiceType.VendorRepeatTx, txType, vendorTxCode, originalResponse)
		{
			ValidateTranactionType(base.TxType, new[] {VspTransactionType.Repeat, VspTransactionType.RepeatDeferred});
		}

		public RepeatResponse Send()
		{
			return new RepeatResponse(this, base.InternalSend());
		}
	}
}