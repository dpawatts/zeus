namespace Protx.Vsp
{
	public class AbortTransaction : VspFollowupTransaction
	{
		public AbortTransaction(VspSecuredResponse originalResponse)
			: base("2.22", VspServiceType.VendorAbortTX, VspTransactionType.Abort, originalResponse)
		{
		}

		public AbortTransaction(string originalVendorTxCode)
			: base("2.22", VspServiceType.VendorAbortTX, VspTransactionType.Abort, originalVendorTxCode)
		{
		}

		public AbortResponse Send()
		{
			return new AbortResponse(this, base.InternalSend());
		}
	}
}