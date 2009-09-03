using Ninject.Modules;

namespace Zeus.AddIns.ECommerce.PaymentGateways.SagePay
{
	public class SagePay : NinjectModule
	{
		public override void Load()
		{
			Bind<IPaymentGateway>().To<SagePayPaymentGateway>().InSingletonScope();
		}
	}
}