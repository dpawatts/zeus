using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeus.AddIns.ECommerce.ContentTypes.Data;
using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.Persistence;
using Zeus.Web;

namespace Zeus.AddIns.ECommerce.Services
{
	public class ShoppingBasketService : IShoppingBasketService
	{
		private readonly IPersister _persister;
		private readonly IWebContext _webContext;
		private readonly IFinder<ShoppingBasket> _finder;

		public ShoppingBasketService(IPersister persister, IWebContext webContext, IFinder<ShoppingBasket> finder)
		{
			_persister = persister;
			_webContext = webContext;
			_finder = finder;
		}

		public void AddItem(Shop shop, Product product)
		{
			ShoppingBasket shoppingBasket = GetCurrentShoppingBasketInternal(shop);

			// If card is already in basket, just increment quantity, otherwise create a new item.
			ShoppingBasketItem item = shoppingBasket.GetChildren<ShoppingBasketItem>().SingleOrDefault(i => i.Product == product);
			if (item == null)
			{
				item = new ShoppingBasketItem { Product = product, Quantity = 1 };
				item.AddTo(shoppingBasket);
			}
			else
			{
				item.Quantity += 1;
			}

			_persister.Save(shoppingBasket);
		}

		public void RemoveItem(Shop shop, Product product)
		{
			UpdateQuantity(shop, product, 0);
		}

		public void UpdateQuantity(Shop shop, Product product, int newQuantity)
		{
			if (newQuantity < 0)
				throw new ArgumentOutOfRangeException("newQuantity", "Quantity must be greater than or equal to 0.");

			ShoppingBasket shoppingBasket = GetCurrentShoppingBasketInternal(shop);
			ShoppingBasketItem item = shoppingBasket.GetChildren<ShoppingBasketItem>().SingleOrDefault(i => i.Product == product);

			if (item == null)
				return;

			if (newQuantity == 0)
				shoppingBasket.Children.Remove(item);
			else
				item.Quantity = newQuantity;

			_persister.Save(shoppingBasket);
		}

		public IShoppingBasket GetBasket(Shop shop)
		{
			return GetCurrentShoppingBasketInternal(shop);
		}

		public void ClearBasket(Shop shop)
		{
			ShoppingBasket shoppingBasket = GetCurrentShoppingBasketInternal(shop);
			if (shoppingBasket != null)
			{
				_persister.Delete(shoppingBasket);
				_webContext.Response.Cookies.Remove(GetCookieKey(shop));
			}
		}

		private static string GetCookieKey(Shop shop)
		{
			return "ZeusECommerce" + shop.ID;
		}

		private ShoppingBasket GetShoppingBasketFromCookie(Shop shop)
		{
			HttpCookie cookie = _webContext.Request.Cookies[GetCookieKey(shop)];
			if (cookie == null)
				return null;

			string shopperID = cookie.Value;
			return _finder.Items().SingleOrDefault(sb => sb.Name == shopperID);
		}

		private ShoppingBasket GetCurrentShoppingBasketInternal(Shop shop)
		{
			ShoppingBasket shoppingBasket = GetShoppingBasketFromCookie(shop);

			if (shoppingBasket != null)
			{
				// Check that products in shopping cart still exist, and if not remove those shopping cart items.
				List<ShoppingBasketItem> itemsToRemove = new List<ShoppingBasketItem>();
				foreach (ShoppingBasketItem item in shoppingBasket.GetChildren<ShoppingBasketItem>())
					if (item.Product == null)
						itemsToRemove.Add(item);
				foreach (ShoppingBasketItem item in itemsToRemove)
					shoppingBasket.Children.Remove(item);
				_persister.Save(shoppingBasket);
			}
			else
			{
				shoppingBasket = new ShoppingBasket { Name = Guid.NewGuid().ToString() };
				shoppingBasket.AddTo(shop.ShoppingBaskets);
				_persister.Save(shoppingBasket);

				HttpCookie cookie = new HttpCookie(GetCookieKey(shop), shoppingBasket.Name)
				{
					Expires = DateTime.Now.AddYears(1)
				};
				_webContext.Response.Cookies.Add(cookie);
			}

			return shoppingBasket;
		}

		/// <summary>
		/// Masks the credit card using XXXXXX and appends the last 4 digits
		/// </summary>
		public string GetMaskedCardNumber(string cardNumber)
		{
			string result = "****";
			if (cardNumber.Length > 8)
			{
				string lastFour = cardNumber.Substring(cardNumber.Length - 4, 4);
				result = "**** **** **** " + lastFour;
			}
			return result;
		}

		public void SaveBasket(Shop shop)
		{
			_persister.Save(GetCurrentShoppingBasketInternal(shop));
		}
	}
}