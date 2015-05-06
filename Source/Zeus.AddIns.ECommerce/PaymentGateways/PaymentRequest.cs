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
        
        public PaymentRequest()
        { }

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

        public PaymentTransactionType TransactionType { get; set; }
        public string TransactionCode { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }

        public Address BillingAddress { get; set; }
        public Address ShippingAddress { get; set; }

        public PaymentCard Card { get; set; }
        public string CardNumber { get; set; }
        public string CardSecurityCode { get; set; }
        
		public string TelephoneNumber { get; set; }
		public string EmailAddress { get; set; }

		public string ClientIpAddress { get; set; }

        public string CurrencyOverride { get; set; }

        public bool CreateToken { get; set; }
        public bool UseToken { get; set; }
        public string Token { get; set; }
        public bool StoreToken { get; set; }
	}
}