using System.IO;

namespace Protx.Vsp
{
	public class VspFollowupTransaction : VspTransaction
	{
		protected internal VspFollowupTransaction(string version, VspServiceType serviceType, VspTransactionType txType,
		                                          VspSecuredResponse originalResponse)
			: base(version, serviceType, txType, originalResponse.Transaction.VendorTxCode)
		{
			VPSTxID = originalResponse.VPSTxID;
			SecurityKey = originalResponse.SecurityKey;
			TxAuthNo = originalResponse.TxAuthNo;
		}

		protected internal VspFollowupTransaction(string version, VspServiceType serviceType, VspTransactionType txType,
		                                          string vendorTxCode) : base(version, serviceType, txType, vendorTxCode)
		{
		}

		protected override Stream InternalSend()
		{
			base.Parameters.RequirePreviousTransaction();
			return base.InternalSend();
		}

		public string SecurityKey
		{
			get { return base.Parameters.SecurityKey; }
			set { base.Parameters.SecurityKey = value; }
		}

		public string TxAuthNo
		{
			get { return base.Parameters.TxAuthNo; }
			set { base.Parameters.TxAuthNo = value; }
		}

		public string VPSTxID
		{
			get { return base.Parameters.VPSTxID; }
			set { base.Parameters.VPSTxID = value; }
		}
	}
}