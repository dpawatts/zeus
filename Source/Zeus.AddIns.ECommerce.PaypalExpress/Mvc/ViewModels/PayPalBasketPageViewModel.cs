using Zeus.Web.Mvc.ViewModels;
using Zeus.AddIns.ECommerce.PaypalExpress.Mvc.ContentTypeInterfaces;
using System;

namespace Zeus.AddIns.ECommerce.PaypalExpress.Mvc.ViewModels
{
    public class PayPalBasketPageViewModel<T> : ViewModel<T>, Zeus.AddIns.ECommerce.PaypalExpress.Mvc.ViewModels.IPayPalBasketPageViewModel where T : ContentItem, IPayPalBasketPage
    {
        public PayPalBasketPageViewModel(T currentItem)
            : base(currentItem)
        {
            
        }

        public string OrderProcessingErrorMessage { get; set; }
        public string PaymentReturnMessage { get; set; }
        public string CheckoutMessage { get; set; }
        public Address ShippingAddress { get; set; }
        public string NoteToSeller { get; set; }
    }
}
