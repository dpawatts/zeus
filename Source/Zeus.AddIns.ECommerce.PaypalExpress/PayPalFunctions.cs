using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Linq;
using Zeus;
using Zeus.Templates.ContentTypes.ReferenceData;
using System.Collections.Generic;
using Zeus.AddIns.ECommerce.PaypalExpress;
using Zeus.AddIns.ECommerce.PaypalExpress.Mvc.ContentTypeInterfaces;

/// <summary>
/// Summary description for NVPAPICaller
/// </summary>
public class NVPAPICaller
{
    //private static readonly ILog log = LogManager.GetLogger(typeof(NVPAPICaller));

    private string pendpointurl = "https://api-3t.paypal.com/nvp";
    private const string CVV2 = "CVV2";

    private const string SIGNATURE = "SIGNATURE";
    private const string PWD = "PWD";
    private const string ACCT = "ACCT";

    //Replace <API_USERNAME> with your API Username
    //Replace <API_PASSWORD> with your API Password
    //Replace <API_SIGNATURE> with your Signature

    /* US Test Settings, or real settings - set in config */
    public string APIUsername = StartPage.UseTestEnvironment ? StartPage.TestAPIUsername : StartPage.APIUsername;
    private string APIPassword = StartPage.UseTestEnvironment ? StartPage.TestAPIPassword : StartPage.APIPassword;
    private string APISignature = StartPage.UseTestEnvironment ? StartPage.TestAPISignature : StartPage.APISignature;

    private static IStartPageForPayPal _startPage { get; set; }
    private static IStartPageForPayPal StartPage
    {
        get
        {
        if (_startPage == null)
            _startPage = Global.StartPage;
        return _startPage;
    } }

    private string Subject = "";
    private string BNCode = "PP-ECWizard";

    //HttpWebRequest Timeout specified in milliseconds 
    private const int Timeout = 100000;
    private static readonly string[] SECURED_NVPS = new string[] { ACCT, CVV2, SIGNATURE, PWD };

    public void TakeCardPayment(
        string cardType,
        string cardNumber,
        int startMonth,
        int startYear,
        int expMonth,
        int expYear,
        int CVV2,
        string firstName,
        string lastName,
        string payerName,
        string street1,
        string street2,
        string city,
        string state,
        string postCode,
        string country,
        decimal amount,
        string currency,
        string ipAddress,
        string paymentType)
    {
        /*
        CallerServices caller = new CallerServices();
        IAPIProfile profile = ProfileFactory.createSignatureAPIProfile();
        profile.APIUsername = APIUsername;
        profile.APIPassword = APIPassword;
        profile.Environment = StartPage.UseTestEnvironment ? "sandbox" : "live";
        profile.Subject = "";
        profile.APISignature = APISignature;
        caller.APIProfile = profile;
        com.paypal.soap.api.DoDirectPaymentRequestDetailsType directPaymentDetails = new com.paypal.soap.api.DoDirectPaymentRequestDetailsType();

        new PayPal.DefaultSOAPAPICallHandler()

        //Set Credit Card
        com.paypal.soap.api.CreditCardDetailsType cc = new com.paypal.soap.api.CreditCardDetailsType();
        cc.CreditCardType = cardType;
        cc.CreditCardNumber = cardNumber;
        cc.StartMonth = startMonth;
        cc.StartMonthSpecified = true;
        cc.StartYear = startYear;
        cc.StartYearSpecified = true;
        cc.ExpMonth = expMonth;
        cc.ExpMonthSpecified = true;
        cc.ExpYear = expYear;
        cc.ExpYearSpecified = true;
        cc.CVV2 = CVV2.ToString();
        //Set Credit Card.CardOwner
        com.paypal.soap.api.PayerInfoType theCardOwner = new com.paypal.soap.api.PayerInfoType();
        com.paypal.soap.api.PersonNameType thePayerName = new com.paypal.soap.api.PersonNameType();
        thePayerName.FirstName = firstName;
        thePayerName.LastName = lastName;
        theCardOwner.PayerName = thePayerName;
        com.paypal.soap.api.AddressType theAddress = new com.paypal.soap.api.AddressType();
        theAddress.Street1 = street1;
        theAddress.Street2 = street2;
        theAddress.CityName = city;
        theAddress.StateOrProvince = state;
        theAddress.PostalCode = postCode;
        theAddress.Country = country;
        theCardOwner.Address = theAddress;
        cc.CardOwner = theCardOwner;
        directPaymentDetails.CreditCard = cc;
        //Set Order Total
        com.paypal.soap.api.BasicAmountType temp = new com.paypal.soap.api.BasicAmountType();
        com.paypal.soap.api.PaymentDetailsType payDetailType = new com.paypal.soap.api.PaymentDetailsType();
        temp.Value = string.Format("{0:0.00}", amount);
        temp.currencyID = currency;
        payDetailType.OrderTotal = temp;
        directPaymentDetails.PaymentDetails = payDetailType;
        //Set IP
        directPaymentDetails.IPAddress = ipAddress;
        //Set Transaction Type
        directPaymentDetails.PaymentAction = paymentType;

        //Set Request
        com.paypal.soap.api.DoDirectPaymentRequestType request = new com.paypal.soap.api.DoDirectPaymentRequestType();

        //Set Payment Detail
        request.DoDirectPaymentRequestDetails = directPaymentDetails;

        request.DoDirectPaymentRequestDetails.CreditCard.CardOwner.Address.CountrySpecified = true;
        return (com.paypal.soap.api.DoDirectPaymentResponseType)caller.Call("DoDirectPayment", request);
        */
    }

    /// <summary>
    /// Sets the API Credentials
    /// </summary>
    /// <param name="Userid"></param>
    /// <param name="Pwd"></param>
    /// <param name="Signature"></param>
    /// <returns></returns>
    public void SetCredentials(string Userid, string Pwd, string Signature)
    {
        APIUsername = Userid;
        APIPassword = Pwd;
        APISignature = Signature;
    }

    /// <summary>
    /// ShortcutExpressCheckout: The method that calls SetExpressCheckout API
    /// </summary>
    /// <param name="amt"></param>
    /// <param ref name="token"></param>
    /// <param ref name="retMsg"></param>
    /// <returns></returns>

    public bool ShortcutExpressCheckout(string amt, ref string token, ref string retMsg, string returnURL, string cancelURL, List<PayPalItem> items, decimal shippingCost, string currency)
    {
        return ShortcutExpressCheckout(amt, ref token, ref retMsg, returnURL, cancelURL, items, shippingCost, currency, false);
    }

    public bool ShortcutExpressCheckout(string amt, ref string token, ref string retMsg, string returnURL, string cancelURL, List<PayPalItem> items, decimal shippingCost, string currency, bool forceReturnURLsOverHttps)
    {
        string host = "www.paypal.com";
        if (StartPage.UseTestEnvironment)
        {
            pendpointurl = "https://api-3t.sandbox.paypal.com/nvp";
            host = "www.sandbox.paypal.com";
        }

        string siteRoot = ConfigurationManager.AppSettings["SiteRoot"];

        returnURL = "http://" + returnURL;
        cancelURL = "http://" + cancelURL;

        //System.Web.HttpContext.Current.Response.Write(returnURL + "<br/>");
        //System.Web.HttpContext.Current.Response.Write(cancelURL + "<br/>");
        //System.Web.HttpContext.Current.Response.End();

        NVPCodec encoder = new NVPCodec();
        encoder["METHOD"] = "SetExpressCheckout";
        encoder["RETURNURL"] = returnURL;
        encoder["CANCELURL"] = cancelURL;
        
        if (!StartPage.UseShipping)
            encoder["NOSHIPPING"] = "1";

        encoder["PAYMENTREQUEST_0_AMT"] = amt;
        encoder["PAYMENTREQUEST_0_PAYMENTACTION"] = "Sale";
        //as uk only
        encoder["PAYMENTREQUEST_0_CURRENCYCODE"] = string.IsNullOrEmpty(currency) ? "GBP" : currency;

        encoder["PAYMENTREQUEST_0_SHIPPINGAMT"] = shippingCost.ToString("0.00");
        encoder["PAYMENTREQUEST_0_HANDLINGAMT"] = "0.00";
        encoder["PAYMENTREQUEST_0_TAXAMT"] = "0.00";
        encoder["PAYMENTREQUEST_0_DESC"] = "Order";

        //add the items from the basket and tell PayPal about them...
        encoder["PAYMENTREQUEST_0_ITEMAMT"] = items.Sum(i => i.Amount).ToString("0.00");

        // item vars        
        int counter = 0;
        foreach (PayPalItem item in items)
        {
            encoder["L_PAYMENTREQUEST_0_NAME" + counter] = item.Name;
            encoder["L_PAYMENTREQUEST_0_DESC" + counter] = item.Description;
            encoder["L_PAYMENTREQUEST_0_AMT" + counter] = item.Amount.ToString("0.00");
            encoder["L_PAYMENTREQUEST_0_QTY" + counter] = item.Quantity.ToString("0.00");
            //encoder["L_PAYMENTREQUEST_0_ITEMURL" + counter] = item.Url;
            counter++;
        }

        string pStrrequestforNvp = encoder.Encode();
        string pStresponsenvp = HttpCall(pStrrequestforNvp);

        NVPCodec decoder = new NVPCodec();
        decoder.Decode(pStresponsenvp);

        string strAck = decoder["ACK"].ToLower();
        if (strAck != null && (strAck == "success" || strAck == "successwithwarning"))
        {
            token = decoder["TOKEN"];

            string ECURL = "https://" + host + "/cgi-bin/webscr?cmd=_express-checkout" + "&token=" + token;

            retMsg = ECURL;
            return true;
        }
        else
        {
            retMsg = "ErrorCode=" + decoder["L_ERRORCODE0"] + "&" +
                "Desc=" + decoder["L_SHORTMESSAGE0"] + "&" +
                "Desc2=" + decoder["L_LONGMESSAGE0"];

            return false;
        }
    }

    /// <summary>
    /// GetShippingDetails: The method that calls SetExpressCheckout API, invoked from the 
    /// Billing Page EC placement
    /// </summary>
    /// <param name="token"></param>
    /// <param ref name="retMsg"></param>
    /// <returns></returns>
    public bool GetShippingDetails(string token, ref string PayerId, ref Address ShippingAddress, ref string noteToSeller, ref string retMsg)
    {
        if (StartPage.UseTestEnvironment)
        {
            pendpointurl = "https://api-3t.sandbox.paypal.com/nvp";
        }

        NVPCodec encoder = new NVPCodec();
        encoder["METHOD"] = "GetExpressCheckoutDetails";
        encoder["TOKEN"] = token;

        string pStrrequestforNvp = encoder.Encode();
        string pStresponsenvp = HttpCall(pStrrequestforNvp);

        NVPCodec decoder = new NVPCodec();
        decoder.Decode(pStresponsenvp);

        string strAck = decoder["ACK"].ToLower();
        if (strAck != null && (strAck == "success" || strAck == "successwithwarning"))
        {
            ShippingAddress.FirstName = decoder["FIRSTNAME"];
            ShippingAddress.Surname = decoder["LASTNAME"];
            ShippingAddress.PersonTitle = decoder["PAYMENTREQUEST_0_SHIPTONAME"];
            ShippingAddress.AddressLine1 = decoder["PAYMENTREQUEST_0_SHIPTOSTREET"];
            ShippingAddress.AddressLine2 = decoder["PAYMENTREQUEST_0_SHIPTOSTREET2"];
            ShippingAddress.TownCity = decoder["PAYMENTREQUEST_0_SHIPTOCITY"];
            ShippingAddress.StateRegion = decoder["PAYMENTREQUEST_0_SHIPTOSTATE"];
            ShippingAddress.Postcode = decoder["PAYMENTREQUEST_0_SHIPTOZIP"];
            //ShippingAddress.Country = decoder["PAYMENTREQUEST_0_SHIPTOCOUNTRYCODE"];
            ShippingAddress.PhoneNumber = decoder["PAYMENTREQUEST_0_SHIPTOPHONENUM"];
            ShippingAddress.Country = decoder["SHIPTOCOUNTRYNAME"];
            ShippingAddress.Email = decoder["EMAIL"];
            noteToSeller = decoder["PAYMENTREQUEST_0_NOTETEXT"];
            return true;
        }
        else
        {
            retMsg = "ErrorCode=" + decoder["L_ERRORCODE0"] + "&" +
                "Desc=" + decoder["L_SHORTMESSAGE0"] + "&" +
                "Desc2=" + decoder["L_LONGMESSAGE0"];

            return false;
        }
    }

    /// <summary>
    /// ConfirmPayment: The method that calls SetExpressCheckout API, invoked from the 
    /// Billing Page EC placement
    /// </summary>
    /// <param name="token"></param>
    /// <param ref name="retMsg"></param>
    /// <returns></returns>
    public bool ConfirmPayment(string finalPaymentAmount, string token, string payerId, ref NVPCodec decoder, ref string retMsg, string currency)
    {
        if (StartPage.UseTestEnvironment)
        {
            pendpointurl = "https://api-3t.sandbox.paypal.com/nvp";
        }

        NVPCodec encoder = new NVPCodec();
        encoder["METHOD"] = "DoExpressCheckoutPayment";
        encoder["TOKEN"] = token;
        encoder["PAYMENTREQUEST_0_PAYMENTACTION"] = "Sale";
        encoder["PAYERID"] = payerId;
        encoder["PAYMENTREQUEST_0_AMT"] = finalPaymentAmount;
        //encoder["CURRENCYCODE"] = SoundInTheory.CatchTheLingo.Web.Classes.Currency.GetPayPalCurrencyCodeAsString();
        encoder["PAYMENTREQUEST_0_CURRENCYCODE"] = string.IsNullOrEmpty(currency) ? "GBP" : currency;

        string pStrrequestforNvp = encoder.Encode();
        string pStresponsenvp = HttpCall(pStrrequestforNvp);

        decoder = new NVPCodec();
        decoder.Decode(pStresponsenvp);

        string strAck = decoder["ACK"].ToLower();
        if (strAck != null && (strAck == "success" || strAck == "successwithwarning"))
        {
            return true;
        }
        else
        {
            retMsg = "ErrorCode=" + decoder["L_ERRORCODE0"] + "&" +
                "Desc=" + decoder["L_SHORTMESSAGE0"] + "&" +
                "Desc2=" + decoder["L_LONGMESSAGE0"];

            return false;
        }
    }

    /// <summary>
    /// HttpCall: The main method that is used for all API calls
    /// </summary>
    /// <param name="NvpRequest"></param>
    /// <returns></returns>
    public string HttpCall(string NvpRequest) //CallNvpServer
    {
        string url = pendpointurl;

        //To Add the credentials from the profile
        string strPost = NvpRequest + "&" + buildCredentialsNVPString();
        strPost = strPost + "&BUTTONSOURCE=" + System.Web.HttpContext.Current.Server.UrlEncode(BNCode);

        HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);
        objRequest.Timeout = Timeout;
        objRequest.Method = "POST";
        objRequest.ContentLength = strPost.Length;

        try
        {
            using (StreamWriter myWriter = new StreamWriter(objRequest.GetRequestStream()))
            {
                myWriter.Write(strPost);
            }
        }
        catch (Exception e)
        {
            /*
            if (log.IsFatalEnabled)
            {
                log.Fatal(e.Message, this);
            }*/
        }

        //Retrieve the Response returned from the NVP API call to PayPal
        HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
        string result;
        using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
        {
            result = sr.ReadToEnd();
        }

        //Logging the response of the transaction
        /* if (log.IsInfoEnabled)
         {
             log.Info("Result :" +
                       " Elapsed Time : " + (DateTime.Now - startDate).Milliseconds + " ms" +
                      result);
         }
         */
        return result;
    }

    /// <summary>
    /// Credentials added to the NVP string
    /// </summary>
    /// <param name="profile"></param>
    /// <returns></returns>
    private string buildCredentialsNVPString()
    {
        NVPCodec codec = new NVPCodec();

        if (!IsEmpty(APIUsername))
            codec["USER"] = APIUsername;

        if (!IsEmpty(APIPassword))
            codec[PWD] = APIPassword;

        if (!IsEmpty(APISignature))
            codec[SIGNATURE] = APISignature;

        if (!IsEmpty(Subject))
            codec["SUBJECT"] = Subject;

        codec["VERSION"] = "63.0";

        return codec.Encode();
    }

    /// <summary>
    /// Returns if a string is empty or null
    /// </summary>
    /// <param name="s">the string</param>
    /// <returns>true if the string is not null and is not empty or just whitespace</returns>
    public static bool IsEmpty(string s)
    {
        return s == null || s.Trim() == string.Empty;
    }
}


public sealed class NVPCodec : NameValueCollection
{
    private const string AMPERSAND = "&";
    private const string EQUALS = "=";
    private static readonly char[] AMPERSAND_CHAR_ARRAY = AMPERSAND.ToCharArray();
    private static readonly char[] EQUALS_CHAR_ARRAY = EQUALS.ToCharArray();

    /// <summary>
    /// Returns the built NVP string of all name/value pairs in the Hashtable
    /// </summary>
    /// <returns></returns>
    public string Encode()
    {
        StringBuilder sb = new StringBuilder();
        bool firstPair = true;
        foreach (string kv in AllKeys)
        {
            string name = UrlEncode(kv);
            string value = UrlEncode(this[kv]);
            if (!firstPair)
            {
                sb.Append(AMPERSAND);
            }
            sb.Append(name).Append(EQUALS).Append(value);
            firstPair = false;
        }
        return sb.ToString();
    }

    /// <summary>
    /// Decoding the string
    /// </summary>
    /// <param name="nvpstring"></param>
    public void Decode(string nvpstring)
    {
        Clear();
        foreach (string nvp in nvpstring.Split(AMPERSAND_CHAR_ARRAY))
        {
            string[] tokens = nvp.Split(EQUALS_CHAR_ARRAY);
            if (tokens.Length >= 2)
            {
                string name = UrlDecode(tokens[0]);
                string value = UrlDecode(tokens[1]);
                Add(name, value);
            }
        }
    }

    private static string UrlDecode(string s) { return HttpUtility.UrlDecode(s); }
    private static string UrlEncode(string s) { return HttpUtility.UrlEncode(s); }

    #region Array methods
    public void Add(string name, string value, int index)
    {
        this.Add(GetArrayName(index, name), value);
    }

    public void Remove(string arrayName, int index)
    {
        this.Remove(GetArrayName(index, arrayName));
    }

    /// <summary>
    /// 
    /// </summary>
    public string this[string name, int index]
    {
        get
        {
            return this[GetArrayName(index, name)];
        }
        set
        {
            this[GetArrayName(index, name)] = value;
        }
    }

    private static string GetArrayName(int index, string name)
    {
        if (index < 0)
        {
            throw new ArgumentOutOfRangeException("index", "index can not be negative : " + index);
        }
        return name + index;
    }
    #endregion



}

public class PayPalItem
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public decimal Quantity { get; set; }
    public string Url { get; set; }
}