namespace Zeus.AddIns.ECommerce.PaymentGateways
{
	public interface IPaymentGatewayService
	{
		IPaymentGateway GetCurrent();
	}
}