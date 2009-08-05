using System;

namespace Protx.Vsp
{
	public class DirectTransaction : VspCardDetailedTransaction
	{
		public DirectTransaction() : this(Guid.NewGuid().ToString(), VspTransactionType.Payment)
		{
		}

		public DirectTransaction(string vendorTxCode) : this(vendorTxCode, VspTransactionType.Payment)
		{
		}

		public DirectTransaction(string vendorTxCode, VspTransactionType txType)
			: base("2.22", VspServiceType.VspDirectTX, txType, vendorTxCode)
		{
			VspTransactionType[] allowed = new VspTransactionType[3];
			allowed[1] = VspTransactionType.Deferred;
			allowed[2] = VspTransactionType.PreAuth;
			ValidateTranactionType(base.TxType, allowed);
		}

		public DirectResponse Send()
		{
			return new DirectResponse(this, base.InternalSend());
		}

		[Obsolete("Use BillingAddress instead")]
		public string Address
		{
			get { return base.Parameters.BillingAddress; }
			set { base.Parameters.BillingAddress = value; }
		}

		public ApplyChecksFlag ApplyAvsCv2
		{
			get { return base.Parameters.ApplyAvsCv2; }
			set { base.Parameters.ApplyAvsCv2 = value; }
		}

		public string CAVV
		{
			get { return base.Parameters.CAVV; }
			set { base.Parameters.CAVV = value; }
		}

		public string ClientIPAddress
		{
			get { return base.Parameters.ClientIPAddress; }
			set { base.Parameters.ClientIPAddress = value; }
		}

		[Obsolete("No longer supported in protocol version 2.22", true)]
		public int ClientNumber
		{
			get { return 0; }
			set { }
		}

		public string ECI
		{
			get { return base.Parameters.ECI; }
			set { base.Parameters.ECI = value; }
		}

		[Obsolete("Use BillingPostCode instead")]
		public string PostCode
		{
			get { return base.Parameters.BillingPostCode; }
			set { base.Parameters.BillingPostCode = value; }
		}

		public string ThreeDSecureStatus
		{
			get { return base.Parameters.ThreeDSecureStatus; }
			set { base.Parameters.ThreeDSecureStatus = value; }
		}

		public string XID
		{
			get { return base.Parameters.XID; }
			set { base.Parameters.XID = value; }
		}
	}
}