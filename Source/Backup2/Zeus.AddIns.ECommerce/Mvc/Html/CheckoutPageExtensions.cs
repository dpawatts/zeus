using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.AddIns.ECommerce.Services;
using Zeus.BaseLibrary.ExtensionMethods;

namespace Zeus.AddIns.ECommerce.Mvc.Html
{
	public static class CheckoutPageExtensions
	{
		public static IEnumerable<SelectListItem> PaymentCardTypes(this HtmlHelper html, object selectedValue)
		{
			return Context.Current.Resolve<IOrderService>().GetSupportedCardTypes().Select(pct => new SelectListItem
			{
				Text = pct.GetDescription(),
				Value = pct.ToString()
			});
		}

		public static IEnumerable<SelectListItem> MonthList(this HtmlHelper html, object selectedValue)
		{
			return Enumerable.Range(1, 12).Select(i => new SelectListItem
			{
				Value = i.ToString(),
				Text = i.ToString().PadLeft(2, '0'),
				Selected = i.Equals(selectedValue)
			});
		}

		public static IEnumerable<SelectListItem> ExpiryYearList(this HtmlHelper html, object selectedValue)
		{
			return Enumerable.Range(DateTime.Now.Year, 15).Select(i => new SelectListItem
			{
				Value = i.ToString(),
				Text = i.ToString(),
				Selected = i.Equals(selectedValue)
			});
		}

		public static IEnumerable<SelectListItem> StartYearList(this HtmlHelper html, object selectedValue)
		{
			return Enumerable.Range(DateTime.Now.Year - 15, 16).Select(i => new SelectListItem
			{
				Value = i.ToString(),
				Text = i.ToString(),
				Selected = i.Equals(selectedValue)
			});
		}

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

			TagBuilder linkTag = new TagBuilder("a");
			linkTag.MergeAttribute("href", shoppingBasketPage.Url, true);
			linkTag.MergeAttribute("id", "basket", true);
			linkTag.SetInnerText(innerText);
			return linkTag.ToString();
		}
	}
}