using Zeus.AddIns.ECommerce.ContentTypes.Pages;

namespace Zeus.AddIns.ECommerce.Services
{
	public interface IShoppingBasketService
	{
		void AddItem(Shop shop, Product product);
		void ClearBasket(Shop shop);
		IShoppingBasket GetBasket(Shop shop);
		void RemoveItem(Shop shop, Product product);
		void UpdateQuantity(Shop shop, Product product, int newQuantity);
	}
}