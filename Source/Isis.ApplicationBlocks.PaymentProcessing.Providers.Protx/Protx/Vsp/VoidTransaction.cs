namespace Protx.Vsp
{
	public class VoidTransaction : VspFollowupTransaction
	{
		public VoidTransaction(VspSecuredResponse originalResponse)
			: base("2.22", VspServiceType.VendorVoidTX, VspTransactionType.Void, originalResponse)
		{
		}

		public VoidTransaction(string originalVendorTxCode)
			: base("2.22", VspServiceType.VendorVoidTX, VspTransactionType.Void, originalVendorTxCode)
		{
		}

		public VoidResponse Send()
		{
			return new VoidResponse(this, base.InternalSend());
		}
	}
}