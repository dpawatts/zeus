using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeus.AddIns.ECommerce.PaypalExpress.Mvc.ContentTypeInterfaces;

namespace Zeus.AddIns.ECommerce.PaypalExpress
{
    public static class Global
    {
        public static IStartPageForPayPal StartPage
        {
            get
            {
                try
                {return Zeus.Find.StartPage as IStartPageForPayPal;}
                catch
                {throw (new Exception("To use the Paypal Express plug in, your StartPage (WebsiteNode) must implement IStartPageForPayPal"));}
            }
        }
    }

    
}
