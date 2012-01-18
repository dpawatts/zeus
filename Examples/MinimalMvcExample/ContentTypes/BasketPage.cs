using Zeus;
using Zeus.Web;
using Zeus.Templates.ContentTypes;
using Zeus.AddIns.ECommerce.PaypalExpress.Mvc.ContentTypeInterfaces;
using System.Collections.Generic;

namespace Zeus.Examples.MinimalMvcExample.ContentTypes
{
    [ContentType("BasketPage")]
    public class BasketPage : BasePage, IPayPalBasketPage
    {
        #region IPayPalBasketPage Members

        public string BasketPagePath
        {
            get { return this.Url; }
        }

        public decimal BasketTotal
        {
            get { return 30.99m + 15.99m + 6.99m; }
        }

        public System.Collections.Generic.List<PayPalItem> BasketItems
        {
            get {
                List<PayPalItem> items = new List<PayPalItem>();
                items.Add(new PayPalItem { Name = "First Item", Amount = 30.99m, Description = "My first item, what a beaut!", Quantity = 1, Url = "" });
                items.Add(new PayPalItem { Name = "Second Item", Amount = 15.99m, Description = "My second item, not so good...", Quantity = 1, Url = "" });
                return items;
            }
        }

        public decimal DeliveryPrice
        {
            get { return 6.99m; }
        }

        public bool ForceCountryMatch
        {
            get { return true; }
        }

        public System.Collections.Generic.IEnumerable<string> PossibleCountries
        {
            get { return new List<string> { "United Kingdom" }; }
        }

        #endregion
    }
}
