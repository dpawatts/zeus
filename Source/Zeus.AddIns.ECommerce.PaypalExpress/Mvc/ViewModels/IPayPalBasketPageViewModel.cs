using System;
namespace Zeus.AddIns.ECommerce.PaypalExpress.Mvc.ViewModels
{
    public interface IPayPalBasketPageViewModel
    {
        string CheckoutMessage { get; set; }
        string OrderProcessingErrorMessage { get; set; }
        string PaymentReturnMessage { get; set; }
        Zeus.AddIns.ECommerce.PaypalExpress.Address ShippingAddress { get; set; }
        string NoteToSeller { get; set; }
    }
}
