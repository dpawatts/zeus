using System;

namespace Isis.ApplicationBlocks.PaymentProcessing
{
	public abstract class PaymentProviderBase
	{
		/// <summary>
		/// Probably only necessary for FAC payment provider.
		/// </summary>
		/// <param name="cardNumber"></param>
		/// <returns></returns>
		public virtual int GetCountryIsoCode(string cardNumber)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Retreieves an HTML string that when rendered produces a page to
		/// authorise the payment transaction specified in the parameters.
		/// </summary>
		public virtual string GetAuthorisationHtml(
			PaymentRequest payment,
			bool isAuthOnly,
			string authorisationUrl,
			string cardNumber,
			DateTime expiryDate,
			string cvv2,
			string billingAddress,
			string billingPostCode)
		{
			throw new NotSupportedException();
		}

		public virtual PaymentProcessingResponse ProcessAuthorizationResponse(string response)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Authorises and banks the supplied payment.
		/// </summary>
		public virtual PaymentResponse AuthAndBank(PaymentRequest payment)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Banks the supplied payment, assuming it has already been authorised.
		/// </summary>
		public virtual void Bank(PaymentRequest payment)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Reverses the supplied payment, assuming it has already been authorised.
		/// </summary>
		public virtual void Reverse(PaymentRequest payment)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Refunds the supplied payment, assuming it has already been authorised.
		/// </summary>
		public virtual void Refund(PaymentRequest payment)
		{
			throw new NotSupportedException();
		}
	}
}