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
            get { return 0.01m + 0.02m + 0.03m; }
        }

        public System.Collections.Generic.List<PayPalItem> BasketItems
        {
            get {
                List<PayPalItem> items = new List<PayPalItem>();
                items.Add(new PayPalItem { Name = "First Item", Amount = 0.01m, Description = "My first item, what a beaut!", Quantity = 1, Url = "" });
                items.Add(new PayPalItem { Name = "Second Item", Amount = 0.02m, Description = "My second item, not so good...", Quantity = 1, Url = "" });
                return items;
            }
        }

        public decimal DeliveryPrice
        {
            get { return 0.03m; }
        }

        public bool ForceCountryMatch
        {
            get { return true; }
        }

        public System.Collections.Generic.IEnumerable<string> PossibleCountries
        {
            get { return new List<string> { "United Kingdom" }; }
        }

        public bool ForceStateMatch
        {
            get { return false; }
        }

        public System.Collections.Generic.IEnumerable<string> PossibleStates
        {
            get { return new List<string>(); }
        }



        #endregion

        #region IPayPalBasketPage Members


        public string Currency
        {
            get { return "GBP"; }
        }

        #endregion
    }
}
