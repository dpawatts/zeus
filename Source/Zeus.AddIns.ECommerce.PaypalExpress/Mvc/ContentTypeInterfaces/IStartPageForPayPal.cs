using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeus.AddIns.ECommerce.PaypalExpress.Mvc.ContentTypeInterfaces
{
    public interface IStartPageForPayPal
    {
        string APIUsername { get; set; }
        string APIPassword { get; set; }
        string APISignature { get; set; }
        string TestAPIUsername { get; set; }
        string TestAPIPassword { get; set; }
        string TestAPISignature { get; set; }
        bool UseTestEnvironment { get; set; }
        bool UseShipping { get; set; }
    }
}
