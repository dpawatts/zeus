namespace Protx.Vsp
{
	public class TransactionData
	{
		public readonly string SecurityKey;
		public readonly object UserData;
		public readonly string VendorTxCode;
		public readonly string VPSTxID;

		public TransactionData(string vendorTxCode, string vpsTxID, string securityKey)
		{
			VendorTxCode = vendorTxCode;
			VPSTxID = vpsTxID;
			SecurityKey = securityKey;
		}

		public TransactionData(string vendorTxCode, string vpsTxID, string securityKey, object userData)
		{
			VendorTxCode = vendorTxCode;
			VPSTxID = vpsTxID;
			SecurityKey = securityKey;
			UserData = userData;
		}
	}
}