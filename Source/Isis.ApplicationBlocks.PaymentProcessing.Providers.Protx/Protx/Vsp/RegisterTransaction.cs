using System;

namespace Protx.Vsp
{
	public class RegisterTransaction : VspMoneyTransaction
	{
		public RegisterTransaction() : this(Guid.NewGuid().ToString(), VspTransactionType.Payment)
		{
		}

		public RegisterTransaction(string vendorTxCode) : this(vendorTxCode, VspTransactionType.Payment)
		{
		}

		public RegisterTransaction(string vendorTxCode, VspTransactionType txType)
			: base("2.22", VspServiceType.VendorRegisterTX, txType, vendorTxCode)
		{
			VspTransactionType[] allowed = new VspTransactionType[3];
			allowed[1] = VspTransactionType.Deferred;
			allowed[2] = VspTransactionType.PreAuth;
			ValidateTranactionType(base.TxType, allowed);
		}

		public RegisterResponse Send()
		{
			return new RegisterResponse(this, base.InternalSend());
		}

		[Obsolete("Use BillingAddress instead")]
		public string Address
		{
			get { return base.Parameters.BillingAddress; }
			set { base.Parameters.BillingAddress = value; }
		}

		public bool AllowGiftAid
		{
			get { return base.Parameters.AllowGiftAid; }
			set { base.Parameters.AllowGiftAid = value; }
		}

		public ApplyChecksFlag Apply3DSecure
		{
			get { return base.Parameters.Apply3DSecure; }
			set { base.Parameters.Apply3DSecure = value; }
		}

		public ApplyChecksFlag ApplyAvsCv2
		{
			get { return base.Parameters.ApplyAvsCv2; }
			set { base.Parameters.ApplyAvsCv2 = value; }
		}

		public string Basket
		{
			get { return base.Parameters.Basket; }
			set { base.Parameters.Basket = value; }
		}

		public string BillingAddress
		{
			get { return base.Parameters.BillingAddress; }
			set { base.Parameters.BillingAddress = value; }
		}

		public string BillingPostCode
		{
			get { return base.Parameters.BillingPostCode; }
			set { base.Parameters.BillingPostCode = value; }
		}

		public string ContactFax
		{
			get { return base.Parameters.ContactFax; }
			set { base.Parameters.ContactFax = value; }
		}

		public string ContactNumber
		{
			get { return base.Parameters.ContactNumber; }
			set { base.Parameters.ContactNumber = value; }
		}

		public string CustomerEMail
		{
			get { return base.Parameters.CustomerEMail; }
			set { base.Parameters.CustomerEMail = value; }
		}

		public string CustomerName
		{
			get { return base.Parameters.CustomerName; }
			set { base.Parameters.CustomerName = value; }
		}

		public string DeliveryAddress
		{
			get { return base.Parameters.DeliveryAddress; }
			set { base.Parameters.DeliveryAddress = value; }
		}

		public string DeliveryPostCode
		{
			get { return base.Parameters.DeliveryPostCode; }
			set { base.Parameters.DeliveryPostCode = value; }
		}

		public string NotificationURL
		{
			get { return base.Parameters.NotificationURL; }
			set { base.Parameters.NotificationURL = value; }
		}

		[Obsolete("Use BillingPostCode instead")]
		public string PostCode
		{
			get { return base.Parameters.BillingPostCode; }
			set { base.Parameters.BillingPostCode = value; }
		}
	}
}