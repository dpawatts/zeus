using System;
using System.IO;

namespace Protx.Vsp
{
	public class VspRelatedTransaction : VspMoneyTransaction
	{
		protected internal VspRelatedTransaction(string version, VspServiceType serviceType, VspTransactionType txType,
		                                         string vendorTxCode) : base(version, serviceType, txType, vendorTxCode)
		{
		}

		protected internal VspRelatedTransaction(string version, VspServiceType serviceType, VspTransactionType txType,
		                                         string vendorTxCode, VspSecuredResponse originalResponse)
			: base(version, serviceType, txType, vendorTxCode)
		{
			RelatedVendorTxCode = originalResponse.Transaction.VendorTxCode;
			VPSTxID = originalResponse.VPSTxID;
			SecurityKey = originalResponse.SecurityKey;
			TxAuthNo = originalResponse.TxAuthNo;
		}

		protected override Stream InternalSend()
		{
			base.Parameters.RequireRelatedFields();
			return base.InternalSend();
		}

		[Obsolete("Use SecurityKey")]
		public string RelatedSecurityKey
		{
			get { return base.Parameters.RelatedSecurityKey; }
			set { base.Parameters.RelatedSecurityKey = value; }
		}

		[Obsolete("Use TxAuthNo")]
		public string RelatedTxAuthNo
		{
			get { return base.Parameters.RelatedTxAuthNo; }
			set { base.Parameters.RelatedTxAuthNo = value; }
		}

		public string RelatedVendorTxCode
		{
			get { return base.Parameters.RelatedVendorTxCode; }
			set { base.Parameters.RelatedVendorTxCode = value; }
		}

		[Obsolete("Use VPSTxID")]
		public string RelatedVPSTxID
		{
			get { return base.Parameters.RelatedVPSTxID; }
			set { base.Parameters.RelatedVPSTxID = value; }
		}

		public string SecurityKey
		{
			get { return base.Parameters.RelatedSecurityKey; }
			set { base.Parameters.RelatedSecurityKey = value; }
		}

		public string TxAuthNo
		{
			get { return base.Parameters.RelatedTxAuthNo; }
			set { base.Parameters.RelatedTxAuthNo = value; }
		}

		public string VPSTxID
		{
			get { return base.Parameters.RelatedVPSTxID; }
			set { base.Parameters.RelatedVPSTxID = value; }
		}
	}
}