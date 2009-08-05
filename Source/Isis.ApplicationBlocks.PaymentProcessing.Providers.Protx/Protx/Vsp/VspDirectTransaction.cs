using System;

namespace Protx.Vsp
{
	[Obsolete("Replaced with DirectTransaction for naming consistency")]
	public class VspDirectTransaction : DirectTransaction
	{
		public VspDirectTransaction() : this(Guid.NewGuid().ToString(), VspTransactionType.Payment)
		{
		}

		public VspDirectTransaction(string vendorTxCode) : this(vendorTxCode, VspTransactionType.Payment)
		{
		}

		public VspDirectTransaction(string vendorTxCode, VspTransactionType txType) : base(vendorTxCode, txType)
		{
		}

		public new VspDirectResponse Send()
		{
			return new VspDirectResponse(this, base.InternalSend());
		}
	}
}