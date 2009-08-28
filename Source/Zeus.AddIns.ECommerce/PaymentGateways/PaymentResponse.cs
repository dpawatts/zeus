namespace Zeus.AddIns.ECommerce.PaymentGateways
{
	public class PaymentResponse
	{
		public PaymentResponse(bool success)
		{
			Success = success;
		}

		public bool Success { get; private set; }
		public string Message { get; set; }
	}
}