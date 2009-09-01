using Ninject.Modules;
using Zeus.AddIns.ECommerce.PaymentGateways;
using Zeus.AddIns.ECommerce.Services;

namespace Zeus.AddIns.ECommerce
{
	public class MailoutModule : NinjectModule
	{
		public override void Load()
		{
			Bind<IShoppingBasketService>().To<ShoppingBasketService>().InSingletonScope();
			Bind<IPaymentGatewayService>().To<PaymentGatewayService>().InSingletonScope();
		}
	}
}