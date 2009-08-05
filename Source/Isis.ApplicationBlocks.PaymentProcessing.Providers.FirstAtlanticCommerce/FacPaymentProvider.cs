using System;
using System.Linq;
using System.Collections.Specialized;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using Isis.ApplicationBlocks.PaymentProcessing.Providers.FirstAtlanticCommerce.FacWebServices;

namespace Isis.ApplicationBlocks.PaymentProcessing.Providers.FirstAtlanticCommerce
{
	/// <summary>
	/// FAC Payment Processing Service wrapper
	/// </summary>
	public class FacPaymentProvider : PaymentProviderBase
	{
		#region Fields

		private string _webServiceUrl;
		private int _acquirerID, _merchantID;
		private string _merchantPassword;
		private bool _testOnly;
		private FacBinRecord[] _binRecords;

		#endregion

		#region Constructor

		public FacPaymentProvider(NameValueCollection config)
		{
			_webServiceUrl = config["webServiceUrl"];
			_acquirerID = Convert.ToInt32(config["acquirerID"]);
			_merchantID = Convert.ToInt32(config["merchantID"]);
			_merchantPassword = config["merchantPassword"];
			_testOnly = Convert.ToBoolean(config["testMode"]);

			List<FacBinRecord> tempBinRecords = new List<FacBinRecord>();
			foreach (string key in config)
			{
				if (key.StartsWith("bin"))
				{
					string[] binData = config[key].Split(',');
					if (binData.Length == 3)
					{
						FacBinRecord binRecord = new FacBinRecord();
						binRecord.CardNumberPrefix = binData[0];
						binRecord.Name = binData[1];
						binRecord.CountryIsoCode = Convert.ToInt32(binData[2]);

						tempBinRecords.Add(binRecord);
					}
				}
			}
			_binRecords = tempBinRecords.ToArray();
		}

		#endregion

		#region Methods

		#region Payment provider implementation

		/// <summary>
		/// Returns the country ISO code matching the BIN number of the card
		/// number string supplied.  A default may be specified in the
		/// web.config file.  -1 is returned if the BIN number is not
		/// recognised and no default is specified.
		/// </summary>
		public override int GetCountryIsoCode(string cardNumber)
		{
			const int binCode = -1;
			if (cardNumber == null)
				return binCode;

			cardNumber = GetDigitsOnly(cardNumber);
			
			// First, try to find actual match for card number.
			FacBinRecord binRecord = _binRecords.SingleOrDefault(b => !string.IsNullOrEmpty(b.CardNumberPrefix) && b.CardNumberPrefix.StartsWith(cardNumber));

			// Next, find default.
			if (binRecord == null)
				binRecord = _binRecords.Single(b => string.IsNullOrEmpty(b.CardNumberPrefix));

			return binRecord.CountryIsoCode;
		}

		/// <summary>
		/// Retreieves an HTML string that when rendered produces a page to
		/// authorise the payment transaction specified in the parameters.
		/// </summary>
		public override string GetAuthorisationHtml(
			PaymentRequest payment,
			bool isAuthOnly,
			string authorisationUrl,
			string cardNumber,
			DateTime expiryDate,
			string cvv2,
			string billingAddress,
			string billingPostCode)
		{
			// Call the web service
			return GetWebService().Auth3DS(
				false,
				true,
				_acquirerID.ToString(),
				_merchantID.ToString(),
				payment.OrderID,
				payment.GetFormattedAmount(),
				payment.GetFormattedCurrencyCode(),
				payment.GetFormattedAmountExponent().ToString(),
				GetDigitsOnly(cardNumber),
				expiryDate.ToString("MMyy"),
				cvv2,
				"SHA1",
				GetHashCode(payment),
				(isAuthOnly) ? "M" : "A",
				authorisationUrl,
				_testOnly ? "True" : "False",
				billingAddress,
				billingPostCode,
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty);
		}

		public override PaymentProcessingResponse ProcessAuthorizationResponse(string response)
		{
			return ProcessResponse(response);
		}

		/// <summary>
		/// Banks the supplied payment, assuming it has already been authorised.
		/// </summary>
		public override void Bank(PaymentRequest payment)
		{
			ProcessResponse(GetWebService().Capture(
			                	_acquirerID.ToString(),
			                	_merchantID.ToString(),
			                	_merchantPassword,
			                	payment.OrderID,
			                	payment.GetFormattedAmount(),
			                	payment.GetFormattedAmountExponent().ToString()));
		}

		/// <summary>
		/// Reverses the supplied payment, assuming it has already been authorised.
		/// </summary>
		public override void Reverse(PaymentRequest payment)
		{
			ProcessResponse(GetWebService().Reversal(
			                	_acquirerID.ToString(),
			                	_merchantID.ToString(),
			                	_merchantPassword,
			                	payment.OrderID,
			                	payment.GetFormattedAmount(),
			                	payment.GetFormattedAmountExponent().ToString()));
		}

		/// <summary>
		/// Refunds the supplied payment, assuming it has already been authorised.
		/// </summary>
		public override void Refund(PaymentRequest payment)
		{
			ProcessResponse(GetWebService().Refund(
			                	_acquirerID.ToString(),
			                	_merchantID.ToString(),
			                	_merchantPassword,
			                	payment.OrderID,
			                	payment.GetFormattedAmount(),
			                	payment.GetFormattedAmountExponent().ToString()));
		}

		#endregion

		#region Helper methods

		private FACPGWS GetWebService()
		{
			FACPGWS ws = new FACPGWS();
			ws.Url = _webServiceUrl;
			return ws;
		}

		/// <summary>
		/// Strips any non-digit values from a string
		/// </summary>
		private static string GetDigitsOnly(string cardNumber)
		{
			if (cardNumber == null)
				return null;

			string lNewNumber = string.Empty;
			for (int i = 0; i < cardNumber.Length; i++)
			{
				if (cardNumber[i] >= '0' && cardNumber[i] <= '9')
					lNewNumber += cardNumber[i];
			}
			return lNewNumber;
		}

		/// <summary>
		/// Computes and returns a SHA1 hash signature to verify the specified
		/// parameters with this payment configuration.
		/// </summary>
		private string GetHashCode(PaymentRequest payment)
		{
			string key = _merchantPassword + _merchantID + _acquirerID + payment.OrderID + payment.GetFormattedAmount() + payment.GetFormattedCurrencyCode();
			return Convert.ToBase64String(
				new SHA1CryptoServiceProvider().ComputeHash(
					Encoding.UTF8.GetBytes(key)));
		}

		private static PaymentProcessingResponse ProcessResponse(string response)
		{
			NameValueCollection parsedResponse = HttpUtility.ParseQueryString(response);
			int responseCode = Convert.ToInt32(parsedResponse["ResponseCode"]);
			if (responseCode != 1)
				throw new PaymentProcessingException(parsedResponse);
			else
				return new PaymentProcessingResponse { OrderID = parsedResponse["OrderID"] };
		}

		#endregion

		#endregion

		#region Classes

		private class FacBinRecord
		{
			public string CardNumberPrefix;
			public string Name;
			public int CountryIsoCode;
		}

		#endregion
	}
}