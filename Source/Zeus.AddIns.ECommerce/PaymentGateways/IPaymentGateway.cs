namespace Zeus.AddIns.ECommerce.PaymentGateways
{
	public interface IPaymentGateway
	{
		bool SupportsCardType(PaymentCardType cardType);
		PaymentResponse TakePayment(PaymentRequest paymentRequest);
	}
}