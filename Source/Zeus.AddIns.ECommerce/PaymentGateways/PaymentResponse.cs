namespace Zeus.AddIns.ECommerce.PaymentGateways
{
	public class PaymentResponse
	{
		public PaymentResponse(bool success)
		{
			Success = success;
		}

		public bool Success { get; private set; }
		public string Message { get; set; }

        public string MD { 
            get { return System.Web.HttpContext.Current.Session["3DSecure_MD"].ToString(); }
            set { System.Web.HttpContext.Current.Session["3DSecure_MD"] = value; } 
        }
        public string ACSURL {
            get { return System.Web.HttpContext.Current.Session["3DSecure_ACSURL"].ToString(); }
            set { System.Web.HttpContext.Current.Session["3DSecure_ACSURL"] = value; }
        }
        public string PAReq {
            get { return System.Web.HttpContext.Current.Session["3DSecure_PAReq"].ToString(); }
            set { System.Web.HttpContext.Current.Session["3DSecure_PAReq"] = value; }
        }
        public string VendorTxCode
        {
            get { return System.Web.HttpContext.Current.Session["3DSecure_VendorTxCode"].ToString(); }
            set { System.Web.HttpContext.Current.Session["3DSecure_VendorTxCode"] = value; }
        }
        public string CallBackUrl
        {
            get { return System.Web.HttpContext.Current.Session["3DSecure_CallBackUrl"].ToString(); }
            set { System.Web.HttpContext.Current.Session["3DSecure_CallBackUrl"] = value; }
        }
	}
}