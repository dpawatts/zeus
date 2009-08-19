using System.Collections.Generic;

namespace Zeus.AddIns.ECommerce.Services
{
	public interface IShoppingBasket
	{
		IEnumerable<IShoppingBasketItem> Items { get; }
		decimal TotalItemPrice { get; }
	}
}