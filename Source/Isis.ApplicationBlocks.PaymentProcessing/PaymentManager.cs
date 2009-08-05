using System;
using System.Configuration;
using System.Web.Compilation;
using Isis.ApplicationBlocks.PaymentProcessing.Configuration;
using Isis.ExtensionMethods.Configuration;
using StructureMap;
using StructureMap.Attributes;

namespace Isis.ApplicationBlocks.PaymentProcessing
{
	public static class PaymentManager
	{
		static PaymentManager()
		{
			PaymentProcessingSection config = (PaymentProcessingSection) ConfigurationManager.GetSection("soundInTheory/paymentProcessing");
			if (config == null)
				throw new ConfigurationErrorsException("Missing soundInTheory/paymentProcessing config section",
					config.ElementInformation.Source,
					config.ElementInformation.LineNumber);

			ObjectFactory.Initialize(x => x.ForRequestedType<PaymentProviderBase>()
				.CacheBy(InstanceScope.Singleton)
				.TheDefault.Is.OfConcreteType(BuildManager.GetType(config.Provider, true))
				.WithCtorArg("config").EqualTo(config.Settings.ToNameValueCollection()));
		}

		private static PaymentProviderBase Provider
		{
			get { return ObjectFactory.GetInstance<PaymentProviderBase>(); }
		}

		/// <summary>
		/// Probably only necessary for FAC payment provider.
		/// </summary>
		/// <param name="cardNumber"></param>
		/// <returns></returns>
		public static int GetCountryIsoCode(string cardNumber)
		{
			return Provider.GetCountryIsoCode(cardNumber);
		}

		/// <summary>
		/// Retreieves an HTML string that when rendered produces a page to
		/// authorise the payment transaction specified in the parameters.
		/// </summary>
		public static string GetAuthorisationHtml(
			PaymentRequest payment,
			bool isAuthOnly,
			string authorisationUrl,
			string cardNumber,
			DateTime expiryDate,
			string cvv2,
			string billingAddress,
			string billingPostCode)
		{
			return Provider.GetAuthorisationHtml(payment, isAuthOnly, authorisationUrl, cardNumber, expiryDate, cvv2, billingAddress, billingPostCode);
		}

		/// <summary>
		/// Banks the supplied payment, assuming it has already been authorised.
		/// </summary>
		public static PaymentProcessingResponse ProcessAuthorizationResponse(string response)
		{
			return Provider.ProcessAuthorizationResponse(response);
		}

		/// <summary>
		/// Banks the supplied payment, assuming it has already been authorised.
		/// </summary>
		public static void Bank(PaymentRequest payment)
		{
			Provider.Bank(payment);
		}

		/// <summary>
		/// Reverses the supplied payment, assuming it has already been authorised.
		/// </summary>
		public static void Reverse(PaymentRequest payment)
		{
			Provider.Reverse(payment);
		}

		/// <summary>
		/// Refunds the supplied payment, assuming it has already been authorised.
		/// </summary>
		public static void Refund(PaymentRequest payment)
		{
			Provider.Refund(payment);
		}
	}
}