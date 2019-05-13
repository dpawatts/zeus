using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;
using System.Text;
using Zeus.AddIns.ECommerce.PaymentGateways.SagePay.Configuration;
using Zeus.BaseLibrary.ExtensionMethods;
using Zeus.BaseLibrary.ExtensionMethods.Collections;
using System.Linq;

namespace Zeus.AddIns.ECommerce.PaymentGateways.SagePay
{
	/// <summary>
	/// http://www.sagepay.com/documents/SagePayDIRECTProtocolandIntegrationGuidelines.pdf
	/// </summary>
	public class SagePayPaymentGateway : IPaymentGateway
	{
		private string _vpsProtocol, _vendorName, _currency, _purchaseUrl, _callbackUrlFor3D;

		public SagePayPaymentGateway()
		{
			// Get configuration section.
			SagePaySection configSection = ConfigurationManager.GetSection("zeus.addIns.ecommerce.paymentGateways/sagePay") as SagePaySection;
			if (configSection == null)
				configSection = new SagePaySection();
			_vpsProtocol = configSection.VpsProtocol;
			_vendorName = configSection.VendorName;
			_currency = configSection.Currency;
            _purchaseUrl = configSection.PurchaseUrl;
            _callbackUrlFor3D = configSection.CallbackFor3D;
		}

        public void OverrideVendorName(string newVendor)
        {
            _vendorName = newVendor;
        }

		public bool SupportsCardType(PaymentCardType cardType)
		{
			switch (cardType)
			{
				case PaymentCardType.VisaCredit:
				case PaymentCardType.VisaDebit:
				case PaymentCardType.VisaElectron:
				case PaymentCardType.MasterCard:
				case PaymentCardType.Maestro:
				case PaymentCardType.JcbCard:
				case PaymentCardType.Laser:
				case PaymentCardType.Solo:
                case PaymentCardType.AmericanExpress:
					return true;
				default :
					return false;
			}
		}

		public PaymentResponse TakePayment(PaymentRequest paymentRequest)
		{
			WebClient webClient = new WebClient();
			byte[] responseBytes = null;
			try
			{
                NameValueCollection postData = BuildPostData(paymentRequest);
                string test = postData.Join(pd => string.Format("{0} = {1}", pd, postData[pd]), "\n");
                responseBytes = webClient.UploadValues(_purchaseUrl, "POST", postData);
			}
			catch (WebException ex)
			{
				// Check for the most common error... unable to reach the purchase URL.
				return new PaymentResponse(false)
				{
					Message = string.Format("An error has occurred whilst trying to register this transaction. The status is {0}, and the description is '{1}'.", ex.Status, ex.Message)
				};
			}

			// No transport level errors, so the message got to Sage Pay.
			// Analyse the response from Direct to check that everything is okay.
			// Registration results come back in the Status and StatusDetail fields.
			string response = Encoding.ASCII.GetString(responseBytes);
			NameValueCollection responseData = null;
			try
			{
				responseData = ParseResponseData(response);
			}
			catch (Exception ex)
			{
				return new PaymentResponse(false)
				{
					Message = string.Format("An error has occurred whilst trying to read the payment gateway's response. The description is '{0}'.", ex.Message)
				};
			}

			string status = responseData["Status"];
			string statusDetail = responseData["StatusDetail"];

			switch (status)
			{
                case "3DAUTH":
                    return Process3DAuthResponse(status, statusDetail, responseData);                    
				default :
					// If this isn't 3D-Auth, then this is an authorisation result (either successful or otherwise).
					return ProcessAuthorisationResponse(status, statusDetail, responseData);
			}
		}
        
        private PaymentResponse Process3DAuthResponse(string status, string statusDetail, NameValueCollection responseData)
		{
			// This is a 3D-Secure transaction, so we need to redirect the customer to their bank **
            // for authentication.  First get the pertinent information from the response **
            return new PaymentResponse(false)
			{
				Message = "3D",
                MD = responseData["MD"],
                ACSURL = responseData["ACSURL"],
                PAReq = responseData["PAReq"],
                CallBackUrl = _callbackUrlFor3D
			};
		}

		private static PaymentResponse ProcessAuthorisationResponse(string status, string statusDetail, NameValueCollection responseData)
		{
			// Get the results form the POST if they are there.
			/*string vpsTxId = responseData["VPSTxId"];
			string securityKey = responseData["SecurityKey"];
			string txAuthNo = responseData["TxAuthNo"];
			string avsCv2 = responseData["AVSCV2"];
			string addressResult = responseData["AddressResult"];
			string postcodeResult = responseData["PostCodeResult"];
			string cv2Result = responseData["CV2Result"];
			string s3DSecureStatus = responseData["3DSecureStatus"];
			string cavv = responseData["CAVV"];*/

			// Great, the signatures DO match, so we can update the database and redirect the user appropriately.
			bool success;
			string message;
			switch (status)
			{
				case "OK" :
					success = true;
					message = "AUTHORISED - The transaction was successfully authorised with the bank.";
					break;
				case "MALFORMED":
					success = false;
					message = string.Format("MALFORMED - The error details are '{0}'.", statusDetail);
					break;
				case "INVALID":
					success = false;
					message = string.Format("INVALID - The error details are '{0}'.", statusDetail);
					break;
				case "NOTAUTHED":
					success = false;
					message = "DECLINED - The transaction was not authorised by the bank.";
					break;
				case "REJECTED":
					success = false;
					message = "REJECTED - The transaction was failed by your 3D-Secure or AVS/CV2 rule-bases.";
					break;
				case "AUTHENTICATED":
					success = true;
					message = "AUTHENTICATED - The transaction was successfully 3D-Secure Authenticated and can now be Authorised.";
					break;
				case "REGISTERED":
					success = true;
					message = "REGISTERED - The transaction was could not be 3D-Secure Authenticated, but has been registered to be Authorised.";
					break;
				case "ERROR":
					success = false;
					message = string.Format("ERROR - There was an error during the payment process. The error details are '{0}'.", statusDetail);
					break;
				default :
					success = false;
					message = string.Format("UNKNOWN - An unknown status was returned from the payment gateway. The status was '{0}', and the error details are '{1}'.", status, statusDetail);
					break;
			}
			return new PaymentResponse(success)
			{
				Message = message,
                Token = responseData["Token"]
			};
		}

		private static NameValueCollection ParseResponseData(string response)
		{
			string[] responseItems = response.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
			NameValueCollection responseData = new NameValueCollection();
			foreach (string responseItem in responseItems)
			{
				string[] responseItemKeyValue = responseItem.Split(new[] { '=' }, StringSplitOptions.None);
                //deal with = being part of the value
                string val = "";
                for (int i = 1; i <= responseItemKeyValue.GetUpperBound(0); i++)
                {
                    if (i > 1)
                        val += "=";
                    val += responseItemKeyValue[i];
                }
                responseData.Add(responseItemKeyValue[0], val);
			}
			return responseData;
		}

		private NameValueCollection BuildPostData(PaymentRequest paymentRequest)
		{
			NameValueCollection postData = new NameValueCollection();

			// Now to build the Direct POST.  For more details see the Direct Protocol 2.22.
			// NB: Fields potentially containing non ASCII characters are URLEncoded when included in the POST
			postData["VPSProtocol"] = _vpsProtocol;
			postData["TxType"] = GetTransactionType(paymentRequest.TransactionType);
			postData["Vendor"] = _vendorName;
			postData["VendorTxCode"] = paymentRequest.TransactionCode;

			postData["Amount"] = paymentRequest.Amount.ToString("F2"); // Formatted to 2 decimal places with leading digit but no commas or currency symbols
			postData["Currency"] = string.IsNullOrEmpty(paymentRequest.CurrencyOverride) ? _currency : paymentRequest.CurrencyOverride;
			if (!string.IsNullOrEmpty(paymentRequest.Description))
				postData["Description"] = paymentRequest.Description.Left(100); // Up to 100 chars of free format description

            if (paymentRequest.UseToken)
            {
                postData["Token"] = paymentRequest.Token;
                postData["StoreToken"] = "1";
                postData["CV2"] = paymentRequest.CardSecurityCode;			    
            }
            else
            { 
			    postData["CardHolder"] = paymentRequest.Card.NameOnCard;
			    postData["CardNumber"] = paymentRequest.CardNumber;
			    if (paymentRequest.Card.ValidFrom != null)
                    postData["StartDate"] = paymentRequest.Card.ValidFrom.Value.ToString("MMyy");
                postData["ExpiryDate"] = paymentRequest.Card.ValidTo.ToString("MMyy");
                if (!string.IsNullOrEmpty(paymentRequest.Card.IssueNumber))
                    postData["IssueNumber"] = paymentRequest.Card.IssueNumber;
                postData["CV2"] = paymentRequest.CardSecurityCode;
			    postData["CardType"] = GetCardType(paymentRequest.Card.CardType);
                postData["CreateToken"] = paymentRequest.CreateToken ? "1" : "0";
            }

			postData["BillingSurname"] = paymentRequest.BillingAddress.Surname;
			postData["BillingFirstnames"] = paymentRequest.BillingAddress.FirstName;
			postData["BillingAddress1"] = paymentRequest.BillingAddress.AddressLine1;
			if (!string.IsNullOrEmpty(paymentRequest.BillingAddress.AddressLine2))
				postData["BillingAddress2"] = paymentRequest.BillingAddress.AddressLine2;
			postData["BillingCity"] = paymentRequest.BillingAddress.TownCity;
			postData["BillingPostCode"] = paymentRequest.BillingAddress.Postcode;

            if (paymentRequest.BillingAddress.Country != null)
            {
                postData["BillingCountry"] = paymentRequest.BillingAddress.Country.Alpha2;

                if (paymentRequest.BillingAddress.Country.Title == "United States")
                {
                    postData["BillingState"] = paymentRequest.BillingAddress.StateRegion;
                }
            }
            else
                postData["BillingCountry"] = "GB";

			postData["DeliverySurname"] = paymentRequest.ShippingAddress.Surname;
			postData["DeliveryFirstnames"] = paymentRequest.ShippingAddress.FirstName;
			postData["DeliveryAddress1"] = paymentRequest.ShippingAddress.AddressLine1;
			if (!string.IsNullOrEmpty(paymentRequest.ShippingAddress.AddressLine2))
				postData["DeliveryAddress2"] = paymentRequest.ShippingAddress.AddressLine2;
			postData["DeliveryCity"] = paymentRequest.ShippingAddress.TownCity;
			postData["DeliveryPostCode"] = paymentRequest.ShippingAddress.Postcode;
            if (paymentRequest.ShippingAddress.Country != null)
            {
                postData["DeliveryCountry"] = paymentRequest.ShippingAddress.Country.Alpha2;

                if (paymentRequest.ShippingAddress.Country.Title == "United States")
                {
                    postData["DeliveryState"] = paymentRequest.ShippingAddress.StateRegion;
                }
            }
            else
                postData["DeliveryCountry"] = "GB";

			if (!string.IsNullOrEmpty(paymentRequest.TelephoneNumber))
				postData["DeliveryPhone"] = paymentRequest.TelephoneNumber;

			postData["CustomerEMail"] = paymentRequest.EmailAddress;
			// postData["Basket"] = FormatBasket(paymentRequest); // TODO

			// Allow fine control over AVS/CV2 checks and rules by changing this value. 0 is Default.
			// It can be changed dynamically, per transaction, if you wish.  See the Direct Protocol document.
			postData["ApplyAVSCV2"] = "0";

			// Send the IP address of the person entering the card details.
			postData["ClientIPAddress"] = paymentRequest.ClientIpAddress;

			// Allow fine control over 3D-Secure checks and rules by changing this value. 0 is Default.
			// It can be changed dynamically, per transaction, if you wish.  See the Direct Protocol document.
			postData["Apply3DSecure"] = "0";

			// Send the account type to be used for this transaction.  Web sites should us E for e-commerce
			// If you are developing back-office applications for Mail Order/Telephone order, use M
			// If your back office application is a subscription system with recurring transactions, use C
			// Your Sage Pay account MUST be set up for the account type you choose.  If in doubt, use E.
			postData["AccountType"] = "E";

            
			return postData;
		}

        /// <summary>
        /// Get card type
        /// </summary>
        /// <param name="cardType"></param>
        /// <returns></returns>
		private static string GetCardType(PaymentCardType cardType)
		{
			switch (cardType)
			{
				case PaymentCardType.VisaCredit :
					return "VISA";
				case PaymentCardType.VisaDebit :
					return "DELTA";
				case PaymentCardType.VisaElectron :
					return "UKE";
				case PaymentCardType.MasterCard :
					return "MC";
				case PaymentCardType.Maestro :
					return "MAESTRO";
				case PaymentCardType.AmericanExpress :
					return "AMEX";
				case PaymentCardType.DinersClub :
					return "DC";
				case PaymentCardType.JcbCard :
					return "JCB";
				case PaymentCardType.Laser :
					return "LASER";
				case PaymentCardType.Solo :
					return "SOLO";
				default :
					throw new NotSupportedException(string.Format("The card type '{0}' is not currently supported by the SagePay payment gateway.", cardType));
			}
		}

        /// <summary>
        /// Get transaction type
        /// </summary>
        /// <param name="transactionType"></param>
        /// <returns></returns>
        private static string GetTransactionType(PaymentTransactionType transactionType)
        {
            switch (transactionType)
            {
                case PaymentTransactionType.Payment:
                    return "PAYMENT";
                case PaymentTransactionType.Deferred:
                    return "DEFERRED";
                case PaymentTransactionType.Authenticate:
                    return "AUTHENTICATE";
                default:
                    throw new NotSupportedException(string.Format("The transaction type '{0}' is not currently supported by the Direct SagePay payment gateway.", transactionType));
            }
        }
	}
}