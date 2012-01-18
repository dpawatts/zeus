using Zeus;
using Zeus.Web;
using Zeus.Templates.ContentTypes;
using Zeus.AddIns.ECommerce.PaypalExpress.Mvc.ContentTypeInterfaces;

namespace Zeus.Examples.MinimalMvcExample.ContentTypes
{
    [ContentType("StartPage")]
    public class StartPage : WebsiteNode, IStartPageForPayPal
    {
        #region IStartPageForPayPal Members

        [Zeus.ContentProperty("Paypal API Username", 100)]
        public virtual string APIUsername
        {
            get { return GetDetail("APIUsername", default(string)); }
            set { SetDetail("APIUsername", value); }
        }

        [Zeus.ContentProperty("Paypal API Password", 110)]
        public virtual string APIPassword
        {
            get { return GetDetail("APIPassword", default(string)); }
            set { SetDetail("APIPassword", value); }
        }

        [Zeus.ContentProperty("Paypal API Signature", 120)]
        public virtual string APISignature
        {
            get { return GetDetail("APISignature", default(string)); }
            set { SetDetail("APISignature", value); }
        }

        [Zeus.ContentProperty("Paypal Test API Username", 130)]
        public virtual string TestAPIUsername
        {
            get { return GetDetail("TestAPIUsername", default(string)); }
            set { SetDetail("TestAPIUsername", value); }
        }

        [Zeus.ContentProperty("Paypal Test API Password", 140)]
        public virtual string TestAPIPassword
        {
            get { return GetDetail("TestAPIPassword", default(string)); }
            set { SetDetail("TestAPIPassword", value); }
        }

        [Zeus.ContentProperty("Paypal Test API Signature", 150)]
        public virtual string TestAPISignature
        {
            get { return GetDetail("TestAPISignature", default(string)); }
            set { SetDetail("TestAPISignature", value); }
        }

        [Zeus.ContentProperty("Use Paypal Test Environment", 160, Description="Make sure this is NOT checked if you want the site to take real payments")]
        public bool UseTestEnvironment
        {
            get { return GetDetail("UseTestEnvironment", true); }
            set { SetDetail("UseTestEnvironment", value); }
        }

        #endregion
    }
}
