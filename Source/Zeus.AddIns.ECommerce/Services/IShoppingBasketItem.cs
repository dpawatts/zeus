using Zeus.AddIns.ECommerce.ContentTypes.Pages;

namespace Zeus.AddIns.ECommerce.Services
{
	public interface IShoppingBasketItem
	{
		Product Product { get; }
		int Quantity { get; }
		decimal LineTotal { get; }
	}
}