using Zeus.AddIns.ECommerce.ContentTypes;

namespace Zeus.AddIns.ECommerce.Services.PaymentGateways
{
    public interface IPaymentGateway
    {
        void TakePayment(ShoppingCart shoppingCart);
    }
}