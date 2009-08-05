using System.IO;

namespace Protx.Vsp
{
	public class VspCardDetailedTransaction : VspCardTransaction
	{
		protected internal VspCardDetailedTransaction(string version, VspServiceType serviceType, VspTransactionType txType,
		                                              string vendorTxCode) : base(version, serviceType, txType, vendorTxCode)
		{
		}

		protected override Stream InternalSend()
		{
			return base.InternalSend();
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

		public string CV2
		{
			get { return base.Parameters.CV2; }
			set { base.Parameters.CV2 = value; }
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

		public bool GiftAidPayment
		{
			get { return base.Parameters.GiftAidPayment; }
			set { base.Parameters.GiftAidPayment = value; }
		}
	}
}