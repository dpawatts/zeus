using System;
using Zeus.AddIns.ECommerce.ContentTypes.Data;
using Zeus.ContentTypes;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.ECommerce.ContentTypes.Pages
{
	[ContentType(Name = "BaseShop")]
	public class Shop : BasePage, ISelfPopulator
	{
		private const string SHOPPING_BASKETS_NAME = "ShoppingBaskets";

		public ShoppingBasketContainer ShoppingBaskets
		{
			get { return GetChild(SHOPPING_BASKETS_NAME) as ShoppingBasketContainer; }
		}

		void ISelfPopulator.Populate()
		{
			ShoppingBasketContainer shoppingBaskets = new ShoppingBasketContainer
			{
				Name = SHOPPING_BASKETS_NAME,
				Title = "Shopping Baskets"
			};
			shoppingBaskets.AddTo(this);
		}
	}
}