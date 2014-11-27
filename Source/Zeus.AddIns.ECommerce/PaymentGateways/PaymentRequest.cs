using System;
using Zeus.AddIns.ECommerce.ContentTypes.Data;

namespace Zeus.AddIns.ECommerce.PaymentGateways
{
    /// <summary>
    /// Payemnt request class
    /// </summary>
	public class PaymentRequest
	{
        /// <summary>
        /// Constructor
        /// </summary>
        public PaymentRequest(PaymentTransactionType transactionType, string transactionCode, decimal amount, string description,
            Address billingAddress, Address shippingAddress,
            PaymentCard card, string cardNumber, string cardSecurityCode,
            string telephoneNumber, string emailAddress,
            string clientIpAddress)
        {
            Initialise(transactionType, transactionCode, amount, description, billingAddress, shippingAddress,
                card, cardNumber, cardSecurityCode, telephoneNumber, emailAddress, clientIpAddress, null, false, false, null, false);
        }

        public PaymentRequest(PaymentTransactionType transactionType, string transactionCode, decimal amount, string description,
			Address billingAddress, Address shippingAddress, 
            PaymentCard card, string cardNumber, string cardSecurityCode,
			string telephoneNumber, string emailAddress,
			string clientIpAddress, string currencyOverride)
		{
            Initialise(transactionType, transactionCode, amount, description, billingAddress, shippingAddress,
                card, cardNumber, cardSecurityCode, telephoneNumber, emailAddress, clientIpAddress, currencyOverride, false, false, null, false);
		}

        public PaymentRequest(PaymentTransactionType transactionType, string transactionCode, decimal amount, string description,
            Address billingAddress, Address shippingAddress,
            PaymentCard card, string cardNumber, string cardSecurityCode,
            string telephoneNumber, string emailAddress,
            string clientIpAddress, string currencyOverride, bool createToken)
        {
            Initialise(transactionType, transactionCode, amount, description, billingAddress, shippingAddress,
                card, cardNumber, cardSecurityCode, telephoneNumber, emailAddress, clientIpAddress, currencyOverride, createToken, false, null, false);
        }

        public PaymentRequest(PaymentTransactionType transactionType, string transactionCode, decimal amount, string description,
            Address billingAddress, Address shippingAddress,
            PaymentCard card, string cardNumber, string cardSecurityCode,
            string telephoneNumber, string emailAddress,
            string clientIpAddress, string currencyOverride, string token, bool storeToken)
        {
            Initialise(transactionType, transactionCode, amount, description, billingAddress, shippingAddress,
                card, cardNumber, cardSecurityCode, telephoneNumber, emailAddress, clientIpAddress, currencyOverride, false, true, token, storeToken);
        }
        
        private void Initialise(PaymentTransactionType transactionType, string transactionCode, decimal amount, string description,
			Address billingAddress, Address shippingAddress, 
            PaymentCard card, string cardNumber, string cardSecurityCode,
			string telephoneNumber, string emailAddress,
            string clientIpAddress, string currencyOverride, bool createToken, bool useToken, string token, bool storeToken)
        {
            TransactionType = transactionType;
            TransactionCode = transactionCode;
            Amount = amount;
            Description = description;

            BillingAddress = billingAddress;
            ShippingAddress = shippingAddress;

            Card = card;
            CardNumber = cardNumber;
            CardSecurityCode = cardSecurityCode;

            TelephoneNumber = telephoneNumber;
            EmailAddress = emailAddress;

            ClientIpAddress = clientIpAddress;

            CurrencyOverride = currencyOverride;

            CreateToken = createToken;
            UseToken = useToken;
            Token = token;
            StoreToken = storeToken;
        }

        public PaymentTransactionType TransactionType { get; private set; }
        public string TransactionCode { get; private set; }
        public decimal Amount { get; private set; }
        public string Description { get; private set; }

        public Address BillingAddress { get; private set; }
        public Address ShippingAddress { get; private set; }

        public PaymentCard Card { get; private set; }
        public string CardNumber { get; private set; }
        public string CardSecurityCode { get; private set; }
        
		public string TelephoneNumber { get; private set; }
		public string EmailAddress { get; private set; }

		public string ClientIpAddress { get; private set; }

        public string CurrencyOverride { get; private set; }

        public bool CreateToken { get; private set; }
        public bool UseToken { get; private set; }
        public string Token { get; private set; }
        public bool StoreToken { get; private set; }
	}
}