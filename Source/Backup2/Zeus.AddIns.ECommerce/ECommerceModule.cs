using Ninject.Modules;
using Zeus.AddIns.ECommerce.Services;

namespace Zeus.AddIns.ECommerce
{
	public class ECommerceModule : NinjectModule
	{
		public override void Load()
		{
			Bind<IOrderMailService>().To<OrderMailService>().InSingletonScope();
			Bind<IShoppingBasketService>().To<ShoppingBasketService>().InSingletonScope();
			Bind<IOrderService>().To<OrderService>().InSingletonScope();
		}
	}
}