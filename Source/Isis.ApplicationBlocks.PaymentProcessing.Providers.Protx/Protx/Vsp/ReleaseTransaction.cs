namespace Protx.Vsp
{
	public class ReleaseTransaction : VspFollowupTransaction
	{
		public ReleaseTransaction(VspSecuredResponse originalResponse)
			: base("2.22", VspServiceType.VendorReleaseTX, VspTransactionType.Release, originalResponse)
		{
		}

		public ReleaseTransaction(string originalVendorTxCode)
			: base("2.22", VspServiceType.VendorReleaseTX, VspTransactionType.Release, originalVendorTxCode)
		{
		}

		public ReleaseResponse Send()
		{
			return new ReleaseResponse(this, base.InternalSend());
		}
	}
}