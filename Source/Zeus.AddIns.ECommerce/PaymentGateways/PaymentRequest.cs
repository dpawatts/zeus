using System;
using Zeus.AddIns.ECommerce.ContentTypes.Data;

namespace Zeus.AddIns.ECommerce.PaymentGateways
{
	public class PaymentRequest
	{
		public PaymentRequest(Address billingAddress, Address shippingAddress,
			string transactionCode, decimal amount, string description)
		{
			BillingAddress = billingAddress;
			ShippingAddress = shippingAddress;
			TransactionCode = transactionCode;
			Amount = amount;
			Description = description;
		}

		public Address BillingAddress { get; private set; }
		public Address ShippingAddress { get; private set; }
		public string TransactionCode { get; private set; }
		public decimal Amount { get; private set; }
		public string Description { get; private set; }

		public string CardHolder { get; private set; }
		public string CardNumber { get; private set; }

		public DateTime? ValidFrom { get; private set; }
		public DateTime ValidTo { get; private set; }

		public string IssueNumber { get; private set; }
		public string VerificationCode { get; private set; }
		public PaymentCardType CardType { get; private set; }

		public string TelephoneNumber { get; private set; }
		public string EmailAddress { get; private set; }

		public string ClientIpAddress { get; private set; }
	}
}