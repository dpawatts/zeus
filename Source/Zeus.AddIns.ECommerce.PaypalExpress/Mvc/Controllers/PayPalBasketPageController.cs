using System.Web.Mvc;
using Zeus.Templates.Mvc.Controllers;
using Zeus.Web;
using Zeus.AddIns.ECommerce.PaypalExpress.Mvc.ViewModels;
using Zeus.AddIns.ECommerce.PaypalExpress.Mvc.ContentTypeInterfaces;
using System.Linq;
using System;

namespace Zeus.AddIns.ECommerce.PaypalExpress.Mvc.Controllers
{
    /// <summary>
    /// For this to work you need to Implement the following spark files : PayPalConfirmation, CheckoutFailed,  CheckoutSuccessful, EmailFailed, PayPalFailed, PayPalReturnedWrongCountry
    /// </summary>
    /// <typeparam name="T">ContentItem which implements IPayPalBasketPage</typeparam>
    public abstract class PayPalBasketPageController<T> : ZeusController<T> where T : ContentItem, IPayPalBasketPage
    {
        public ActionResult Checkout()
        {
            var basketPageViewModel = GetViewModel(CurrentItem);

            var ReturnUrl = System.Web.HttpContext.Current.Request.Url.Host + CurrentItem.BasketPagePath;

            string token = null;
            string retMsg = null;
            string amt = CurrentItem.BasketTotal.ToString().TrimEnd('0');

            //pass in the amount to take here, as well as the yay (success) and boo (failure) pages
            NVPAPICaller payPalCaller = new NVPAPICaller();
            bool ret = payPalCaller.ShortcutExpressCheckout(amt, ref token, ref retMsg,
                                                            ReturnUrl + "/PayPalConfirmation",
                                                            ReturnUrl + "/CheckoutFailed",
                                                            CurrentItem.BasketItems,
                                                            CurrentItem.DeliveryPrice);
            if (ret)
            {
                
                System.Web.HttpContext.Current.Session["token"] = token;
                System.Web.HttpContext.Current.Session["payment_amt"] = CurrentItem.BasketTotal;
                //this kicks it to the correct PayPal page in order to take the payment
                System.Web.HttpContext.Current.Response.Redirect(retMsg);
                return null;
            }
            else
            {
                basketPageViewModel.PaymentReturnMessage = retMsg;
                //the payment failed
                //code here should display the error message - this is not a payment error, this is an account error
            }

            return View("Index", basketPageViewModel);
        }

        public virtual Type typeOfViewModel { get { return typeof(PayPalBasketPageViewModel<T>); } }

        [HttpGet]
        public ActionResult CheckoutSuccessful()
        {
            NVPAPICaller test = new NVPAPICaller();

            string retMsg = "";
            string token = "";
            string finalPaymentAmount = "";
            string payerId = "";
            NVPCodec decoder = new NVPCodec();

            token = System.Web.HttpContext.Current.Session["ppToken"].ToString();
            payerId = System.Web.HttpContext.Current.Session["ppID"].ToString();
            finalPaymentAmount = CurrentItem.BasketTotal.ToString().TrimEnd('0');

            bool ret = test.ConfirmPayment(finalPaymentAmount, token, payerId, ref decoder, ref retMsg);

            if (ret)
            {
                var basketPageViewModel = GetViewModel(CurrentItem);
                var shippingAddress = (Address)System.Web.HttpContext.Current.Session["shippingAddress"];

                try
                {
                    PayPalOrderSuccess(basketPageViewModel, shippingAddress, token);
                    basketPageViewModel.ShippingAddress = shippingAddress;
                    return View(basketPageViewModel);
                }
                catch (System.Exception ex)
                {
                    // log error in system event viewer
                    //Logger.PayPalCheckoutOrderError(ex, token, Basket.GetBasket(), shippingAddress);

                    // show error view
                    basketPageViewModel.OrderProcessingErrorMessage = ex.Message;
                    return View("EmailFailed", basketPageViewModel);
                }
            }
            else
            {
                var basketPageViewModel = GetViewModel(CurrentItem);
                basketPageViewModel.CheckoutMessage = retMsg;

                return View("PayPalFailed", basketPageViewModel);
            }
        }

        public virtual void PayPalOrderSuccess(IPayPalBasketPageViewModel viewModel, Address shippingAddress, string token)
        {
            //This needs to be overridden...
            /*
            var initBasketItems = basketPageViewModel.ItemsInBasket;

            // create order
            var processor = new CheckoutProcessor();
            var orderReference = processor.ProcessOrder(_basket, shippingAddress, shippingAddress, "PayPalExpress", token, "");

            // send confirmation e-mail
            EmailNotifyer.SendConfirmationEmail(Basket.GetBasket(), shippingAddress);

            // show the view
            basketPageViewModel.OrderReference = orderReference;
            basketPageViewModel.ShippingAddress = shippingAddress;
            return View(basketPageViewModel);
             */
        }
        
        [HttpGet]
        public ActionResult PayPalConfirmation(string token, string PayerID)
        {
            //get shipping address
            NVPAPICaller test = new NVPAPICaller();

            Address shippingAddress = new Address();
            string retMsg = "";
            bool pass = test.GetShippingDetails(token, ref PayerID, ref shippingAddress, ref retMsg);

            if (pass)
            {
                System.Web.HttpContext.Current.Session["shippingAddress"] = shippingAddress;
                System.Web.HttpContext.Current.Session[""] = shippingAddress;
                System.Web.HttpContext.Current.Session["ppToken"] = token;
                System.Web.HttpContext.Current.Session["ppID"] = PayerID;

                //at this point check to see if an appropriate country has been used...

                var basketPageViewModel = GetViewModel(CurrentItem);
                basketPageViewModel.ShippingAddress = shippingAddress;
                
                if (CurrentItem.ForceCountryMatch && !CurrentItem.PossibleCountries.Contains(shippingAddress.Country))
                {
                    //throw the error...
                    return View("PayPalReturnedWrongCountry", basketPageViewModel);
                }

                var result = View(basketPageViewModel);

                return result;
            }
            else
            {
                var basketPageViewModel = GetViewModel(CurrentItem);
                basketPageViewModel.CheckoutMessage = retMsg;

                return View("PayPalFailed", basketPageViewModel);
            }
        }

        public ActionResult CheckoutFailed(string token)
        {
            var basketPageViewModel = GetViewModel(CurrentItem);
            var result = View(basketPageViewModel);
            return result;
        }

        public virtual IPayPalBasketPageViewModel GetViewModel(T CurrentItem)
        {
            return new PayPalBasketPageViewModel<T>(CurrentItem);
        }
    }
}
