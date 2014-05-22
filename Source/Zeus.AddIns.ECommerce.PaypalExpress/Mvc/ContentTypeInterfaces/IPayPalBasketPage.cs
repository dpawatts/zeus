using Zeus;
using Zeus.Web;
using Zeus.Templates.ContentTypes;
using System.Collections.Generic;

namespace Zeus.AddIns.ECommerce.PaypalExpress.Mvc.ContentTypeInterfaces
{
    public interface IPayPalBasketPage
    {
        string BasketPagePath { get; }
        decimal BasketTotal { get; }
        List<PayPalItem> BasketItems { get; }
        decimal DeliveryPrice { get; }
        bool ForceCountryMatch { get; }
        IEnumerable<string> PossibleCountries { get; }
        string Currency { get; }
        bool ForceStateMatch { get; }
        IEnumerable<string> PossibleStates { get; }        
    }
}
