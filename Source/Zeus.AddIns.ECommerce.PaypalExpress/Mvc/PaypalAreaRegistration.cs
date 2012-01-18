using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeus.Web.Mvc;

namespace Zeus.AddIns.ECommerce.PaypalExpress.Mvc
{
    public class PaypalAreaRegistration : StandardAreaRegistration
    {
        public const string AREA_NAME = "Paypal";

        public override string AreaName
        {
            get { return AREA_NAME; }
        }
    }
}
