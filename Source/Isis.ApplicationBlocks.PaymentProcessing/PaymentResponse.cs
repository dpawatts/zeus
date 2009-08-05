namespace Isis.ApplicationBlocks.PaymentProcessing
{
	/// <summary>
	/// Summary description for PaymentResponse.
	/// </summary>
	public class PaymentResponse
	{
		public StatusType Status;
		public string StatusDetail;
		public string TransactionID;
		public string AuthNo;
		public AvsCv2Result AvsCv2;
		public CheckResult AddressResult;
		public CheckResult PostCodeResult;
		public CheckResult Cv2Result;
	}
}
