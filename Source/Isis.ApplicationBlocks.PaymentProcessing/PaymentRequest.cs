using System;

namespace Isis.ApplicationBlocks.PaymentProcessing
{
	/// <summary>
	/// Stores information about a payment
	/// </summary>
	public class PaymentRequest
	{
		#region Constructor

		public PaymentRequest(string orderID, decimal amount, string currencyAlphabeticCode)
		{
			CurrencyAlphabeticCode = currencyAlphabeticCode;
			OrderID = orderID;
			Amount = amount;

			BillingAddress = string.Empty;
			BillingPostcode = string.Empty;
			ClientIPAddress = string.Empty;
		}

		#endregion

		#region Properties

		public string CurrencyAlphabeticCode { get; private set; }
		public string OrderID { get; private set; }
		public decimal Amount { get; private set; }

		public string Description { get; set; }
		public string CardHolder { get; set; }
		public string CardNumber { get; set; }

		public MonthYear? StartDate { get; set; }
		public MonthYear ExpiryDate { get; set; }

		public string IssueNumber { get; set; }
		public string CV2 { get; set; }
		public CardType CardType { get; set; }

		public string BillingAddress { get; set; }
		public string BillingPostcode { get; set; }

		public string ClientIPAddress { get; set; }

		public int CurrencyNumericCode
		{
			get
			{
				switch (CurrencyAlphabeticCode)
				{
					case "BMD" :
						return 060;
					case "GBP" :
						return 826;
					case "USD" :
						return 840;
					default :
						throw new NotImplementedException();
				}
			}
		}

		#endregion

		/// <summary>
		/// Returns the amount formatted in the FAC style
		/// </summary>
		public string GetFormattedAmount()
		{
			return Amount.ToString("0000000000.00").Replace(".", string.Empty);
		}

		/// <summary>
		/// Returns the number of decimal places in the formatted currency amount
		/// </summary>
		public int GetFormattedAmountExponent()
		{
			return 2;
		}

		/// <summary>
		/// Returns the currency code formatted in the FAC style
		/// </summary>
		public string GetFormattedCurrencyCode()
		{
			return CurrencyNumericCode.ToString().PadLeft(3, '0');
		}
	}
}