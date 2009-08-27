using System.Linq;
using System.Web.Mvc;
using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.AddIns.ECommerce.Services;

namespace Zeus.AddIns.ECommerce.Mvc.Html
{
	public static class ShoppingBasketExtensions
	{
		public static string ShoppingBasketSummary(this HtmlHelper html, Shop shop)
		{
			if (shop == null)
				return "[[Shop page not found]]";

			ShoppingBasketPage shoppingBasketPage = shop.GetChild("shopping-basket") as ShoppingBasketPage;
			if (shoppingBasketPage == null)
				return "[[Shopping basket page not found]]";

			IShoppingBasketService shoppingBasketService = Context.Current.Resolve<IShoppingBasketService>();
			IShoppingBasket shoppingBasket = shoppingBasketService.GetBasket(shop);
			string innerText = string.Format("You have <span>{0}</span> items in your shopping basket (<span>{1:C2}</span>)",
				shoppingBasket.TotalItemCount, shoppingBasket.SubTotalPrice);

			return new MvcContrib.UI.Tags.Link
			{
				Href = shoppingBasketPage.Url,
				Id = "basket",
				InnerText = innerText
			}.ToString();
		}
	}
}