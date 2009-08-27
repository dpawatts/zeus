using System.Collections.Generic;
using Zeus.AddIns.ECommerce.ContentTypes.Data;

namespace Zeus.AddIns.ECommerce.Services
{
	public interface IShoppingBasket
	{
		IEnumerable<IShoppingBasketItem> Items { get; }
		DeliveryMethod DeliveryMethod { get; set; }
		int TotalItemCount { get; }
		decimal SubTotalPrice { get; }
		decimal TotalPrice { get; }

		Address BillingAddress { get; }
		Address ShippingAddress { get; }
	}
}