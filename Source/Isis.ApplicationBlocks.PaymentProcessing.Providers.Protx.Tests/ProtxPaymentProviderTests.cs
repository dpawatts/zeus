using System;
using MbUnit.Framework;

namespace Isis.ApplicationBlocks.PaymentProcessing.Providers.Protx.Tests
{
	[TestFixture]
	public class ProtxPaymentProviderTests
	{
		[Test]
		public void Test_VisaDelta_ValidExpiryDate()
		{
			PaymentRequest paymentRequest = new PaymentRequest(Guid.NewGuid().ToString(), 20, "GBP")
			{
				Description = "Test payment",
				CardType = CardType.VisaDelta,
				CardHolder = "Tim Jones",
				CardNumber = "4462000000000003",
				ExpiryDate = new MonthYear { Month = 08, Year = 2009 }
			};
			ProtxPaymentProvider paymentProvider = new ProtxPaymentProvider();
			PaymentResponse paymentResponse = paymentProvider.AuthAndBank(paymentRequest);
			Assert.AreEqual(StatusType.OK, paymentResponse.Status);
		}

		[Test]
		public void Test_VisaDelta_Expired()
		{
			PaymentRequest paymentRequest = new PaymentRequest(Guid.NewGuid().ToString(), 20, "GBP")
			{
				Description = "Test payment",
				CardType = CardType.VisaDelta,
				CardHolder = "Tim Jones",
				CardNumber = "4462000000000003",
				ExpiryDate = new MonthYear { Month = 08, Year = 2008 }
			};
			ProtxPaymentProvider paymentProvider = new ProtxPaymentProvider();
			PaymentResponse paymentResponse = paymentProvider.AuthAndBank(paymentRequest);
			Assert.AreEqual(StatusType.Invalid, paymentResponse.Status);
		}

		[Test]
		public void Test_VisaDelta_InvalidCardNumber()
		{
			PaymentRequest paymentRequest = new PaymentRequest(Guid.NewGuid().ToString(), 20, "GBP")
			{
				Description = "Test payment",
				CardType = CardType.VisaDelta,
				CardHolder = "Tim Jones",
				CardNumber = "1000000000000003",
				ExpiryDate = new MonthYear { Month = 08, Year = 2009 }
			};
			ProtxPaymentProvider paymentProvider = new ProtxPaymentProvider();
			PaymentResponse paymentResponse = paymentProvider.AuthAndBank(paymentRequest);
			Assert.AreEqual(StatusType.Invalid, paymentResponse.Status);
		}
	}
}
