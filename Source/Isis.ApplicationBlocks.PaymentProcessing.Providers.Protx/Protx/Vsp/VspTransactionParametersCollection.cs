using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Protx.Vsp
{
	internal class VspTransactionParametersCollection : NameValueCollection
	{
		private static readonly Regex _regexCV2 = new Regex(@"^\d{3,4}$");
		private static readonly Regex _regexDate = new Regex(@"^[01]\d{3}$");
		private static readonly Regex _regexIssueNumber = new Regex(@"^\d+$");
		private static readonly Regex _regexWhiteSpace = new Regex(@"\s");

		public void CopyTo(VspParameter[] array, int index)
		{
			((ICollection) this).CopyTo(array, index);
		}

		public void DefaultCurrency()
		{
			if (Currency == null)
			{
				Currency = VspConfiguration.GetConfig().DefaultCurrency;
			}
		}

		public void DefaultDescription()
		{
			if (Description == null)
			{
				Description = VspConfiguration.GetConfig().DefaultDescription;
			}
		}

		public void DefaultEmail()
		{
			if ((VendorEMail == null) || (VendorEMail.Length == 0))
			{
				VendorEMail = VspConfiguration.GetConfig().DefaultEmail;
			}
		}

		public static string MakeParamName(VspParameter name)
		{
			return name.ToString("G").Replace("_", "");
		}

		public void Remove(VspParameter name)
		{
			base.Remove(MakeParamName(name));
		}

		protected static string RemoveWhiteSpace(string value)
		{
			return _regexWhiteSpace.Replace(value, "");
		}

		public void Require(VspParameter name)
		{
			if (this[name] == null)
			{
				throw new VspException(string.Format(CultureInfo.InvariantCulture, "{0} must be set before sending transaction",
				                                     new object[] {name}));
			}
		}

		public void RequireAddress()
		{
			if (BillingAddress == null)
			{
				throw new VspException("Address must be set before sending transaction");
			}
			if (BillingPostCode == null)
			{
				throw new VspException("PostCode must be set before sending transaction");
			}
		}

		public void RequireCardFields()
		{
			if (CardHolder == null)
			{
				throw new VspException("CardHolder must be set before sending transaction");
			}
			if (CardNumber == null)
			{
				throw new VspException("CardNumber must be set before sending transaction");
			}
			if (ExpiryDate == null)
			{
				throw new VspException("ExpiryDate must be set before sending transaction");
			}
			if (this[VspParameter.CardType] == null)
			{
				throw new VspException("CardType must be set before sending transaction");
			}
		}

		public void RequirePreviousTransaction()
		{
			if (VPSTxID == null)
			{
				throw new VspException("VPSTxID must be set before sending transaction");
			}
			if (SecurityKey == null)
			{
				throw new VspException("SecurityKey must be set before sending transaction");
			}
			if (TxAuthNo == null)
			{
				throw new VspException("TxAuthNo must be set before sending transaction");
			}
		}

		public void RequireRelatedFields()
		{
			if (RelatedVPSTxID == null)
			{
				throw new VspException("RelatedVPSTxID must be set before sending transaction");
			}
			if (RelatedVendorTxCode == null)
			{
				throw new VspException("RelatedVendorTxCode must be set before sending transaction");
			}
			if (RelatedSecurityKey == null)
			{
				throw new VspException("RelatedSecurityKey must be set before sending transaction");
			}
			if (RelatedTxAuthNo == null)
			{
				throw new VspException("RelatedTxAuthNo must be set before sending transaction");
			}
		}

		protected void SetRequired(string value, VspParameter name)
		{
			if ((value == null) || (value.Length == 0))
			{
				string paramName = name.ToString("G");
				throw new ArgumentNullException(paramName,
				                                string.Format(CultureInfo.InvariantCulture, "{0} MUST be specified",
				                                              new object[] {paramName}));
			}
			this[name] = value;
		}

		protected void SetRequiredWithCheckLength(string value, StringLengths maxLength, VspParameter name)
		{
			if (MyString.IsNullOrEmpty(value))
			{
				string paramName = name.ToString("G");
				throw new ArgumentNullException(paramName,
				                                string.Format(CultureInfo.InvariantCulture, "{0} MUST be specified",
				                                              new object[] {paramName}));
			}
			SetWithCheckLength(value, maxLength, name);
		}

		protected void SetWithCheckLength(string value, StringLengths maxLength, VspParameter name)
		{
			if (value.Length > (int) maxLength)
			{
				string paramName = name.ToString("G");
				throw new ArgumentException(
					string.Format(CultureInfo.InvariantCulture, "{0} must not exceed {1} characters",
					              new object[] {paramName, maxLength.ToString("D", CultureInfo.InvariantCulture)}), paramName);
			}
			this[name] = value;
		}

		protected void ValidateCV2(string value)
		{
			if (!_regexCV2.IsMatch(value))
			{
				throw new ArgumentException("Invalid date", "value");
			}
		}

		protected void ValidateDate(string value)
		{
			if (!_regexDate.IsMatch(value))
			{
				throw new ArgumentException("Invalid date", "value");
			}
		}

		protected void ValidateIssueNumber(string value)
		{
			if (!_regexIssueNumber.IsMatch(value))
			{
				throw new ArgumentException("Invalid date", "value");
			}
		}

		[Obsolete("Use BillingAddress instead", true)]
		public string Address
		{
			get { return this[VspParameter.Address]; }
			set { SetWithCheckLength(value, StringLengths.MaxAddress, VspParameter.Address); }
		}

		public bool AllowGiftAid
		{
			get { return (this[VspParameter.AllowGiftAid] == "1"); }
			set { this[VspParameter.AllowGiftAid] = value ? "1" : "0"; }
		}

		public decimal Amount
		{
			get { return decimal.Parse(this[VspParameter.Amount], CultureInfo.InvariantCulture); }
			set
			{
				if ((value < 0M) || (value > 100000.00M))
				{
					throw new ArgumentOutOfRangeException("value", value, "Amount must be positive");
				}
				if (decimal.Round(value, 2) != value)
				{
					throw new ArgumentException("Amount must contain a maximum of 2 DP");
				}
				this[VspParameter.Amount] = value.ToString("F2", CultureInfo.InvariantCulture);
			}
		}

		public ApplyChecksFlag Apply3DSecure
		{
			get { return (ApplyChecksFlag) Enum.Parse(typeof (ApplyChecksFlag), this[VspParameter.Apply3DSecure], true); }
			set { this[VspParameter.Apply3DSecure] = value.ToString("D"); }
		}

		public ApplyChecksFlag ApplyAvsCv2
		{
			get { return (ApplyChecksFlag) Enum.Parse(typeof (ApplyChecksFlag), this[VspParameter.ApplyAVSCV2], true); }
			set { this[VspParameter.ApplyAVSCV2] = value.ToString("D"); }
		}

		public string AuthCode
		{
			get { return this[VspParameter.AuthCode]; }
			set { SetWithCheckLength(value, StringLengths.MaxIPAddress, VspParameter.AuthCode); }
		}

		public string Basket
		{
			get { return this[VspParameter.Basket]; }
			set { SetWithCheckLength(value, StringLengths.MaxBasket, VspParameter.Basket); }
		}

		public string BillingAddress
		{
			get { return this[VspParameter.BillingAddress]; }
			set { SetWithCheckLength(value, StringLengths.MaxAddress, VspParameter.BillingAddress); }
		}

		public string BillingPostCode
		{
			get { return this[VspParameter.BillingPostCode]; }
			set { SetWithCheckLength(value, StringLengths.MaxSecurityKey, VspParameter.BillingPostCode); }
		}

		public string CardHolder
		{
			get { return this[VspParameter.CardHolder]; }
			set { SetWithCheckLength(value, StringLengths.MaxCardHolder, VspParameter.CardHolder); }
		}

		public string CardNumber
		{
			get { return this[VspParameter.CardNumber]; }
			set { SetWithCheckLength(RemoveWhiteSpace(value), StringLengths.MaxCardNumber, VspParameter.CardNumber); }
		}

		public VspCardType CardType
		{
			get { return (VspCardType) Enum.Parse(typeof (VspCardType), this[VspParameter.CardType], true); }
			set { SetWithCheckLength(value.ToString("G"), StringLengths.MaxIPAddress, VspParameter.CardType); }
		}

		public string CAVV
		{
			get { return this[VspParameter.CAVV]; }
			set { SetWithCheckLength(value, StringLengths.MaxCAVV, VspParameter.CAVV); }
		}

		public string ClientIPAddress
		{
			get { return this[VspParameter.ClientIPAddress]; }
			set { SetWithCheckLength(value, StringLengths.MaxIPAddress, VspParameter.ClientIPAddress); }
		}

		[Obsolete("No longer supported in protocol version 2.22", true)]
		public int ClientNumber
		{
			get { return 0; }
			set { }
		}

		public string ContactFax
		{
			get { return this[VspParameter.ContactFax]; }
			set { SetWithCheckLength(value, StringLengths.MaxCardNumber, VspParameter.ContactFax); }
		}

		public string ContactNumber
		{
			get { return this[VspParameter.ContactNumber]; }
			set { SetWithCheckLength(value, StringLengths.MaxCardNumber, VspParameter.ContactNumber); }
		}

		public string Currency
		{
			get { return this[VspParameter.Currency]; }
			set { SetWithCheckLength(value, StringLengths.MaxCurrency, VspParameter.Currency); }
		}

		public string CustomerEMail
		{
			get { return this[VspParameter.CustomerEMail]; }
			set { SetWithCheckLength(value, StringLengths.MaxEMail, VspParameter.CustomerEMail); }
		}

		public string CustomerName
		{
			get { return this[VspParameter.CustomerName]; }
			set { SetWithCheckLength(value, StringLengths.MaxName, VspParameter.CustomerName); }
		}

		public string CV2
		{
			get { return this[VspParameter.CV2]; }
			set
			{
				if (value != null)
				{
					ValidateCV2(value);
				}
				this[VspParameter.CV2] = value;
			}
		}

		public string DeliveryAddress
		{
			get { return this[VspParameter.DeliveryAddress]; }
			set { SetWithCheckLength(value, StringLengths.MaxAddress, VspParameter.DeliveryAddress); }
		}

		public string DeliveryPostCode
		{
			get { return this[VspParameter.DeliveryPostCode]; }
			set { SetWithCheckLength(value, StringLengths.MaxSecurityKey, VspParameter.DeliveryPostCode); }
		}

		public string Description
		{
			get { return this[VspParameter.Description]; }
			set { SetWithCheckLength(value, StringLengths.MaxName, VspParameter.Description); }
		}

		public string ECI
		{
			get { return this[VspParameter.ECI]; }
			set { SetWithCheckLength(value, StringLengths.MaxIssueNo, VspParameter.ECI); }
		}

		public string eMailMessage
		{
			get { return this[VspParameter.eMailMessage]; }
			set { SetWithCheckLength(value, StringLengths.MaxBasket, VspParameter.eMailMessage); }
		}

		public string ExpiryDate
		{
			get { return this[VspParameter.ExpiryDate]; }
			set
			{
				ValidateDate(value);
				this[VspParameter.ExpiryDate] = value;
			}
		}

		public string FailureURL
		{
			get { return this[VspParameter.FailureURL]; }
			set { SetRequiredWithCheckLength(value, StringLengths.MaxEMail, VspParameter.FailureURL); }
		}

		public bool GiftAidPayment
		{
			get { return (this[VspParameter.GiftAidPayment] == "1"); }
			set { this[VspParameter.GiftAidPayment] = value ? "1" : "0"; }
		}

		public string IssueNumber
		{
			get { return this[VspParameter.IssueNumber]; }
			set
			{
				if (value != null)
				{
					ValidateIssueNumber(value);
				}
				this[VspParameter.IssueNumber] = value;
			}
		}

		public string this[VspParameter name]
		{
			get { return base[MakeParamName(name)]; }
			set
			{
				if (value == null)
				{
					Remove(name);
				}
				else
				{
					base[MakeParamName(name)] = value;
				}
			}
		}

		public string NotificationURL
		{
			get { return this[VspParameter.NotificationURL]; }
			set { SetWithCheckLength(value, StringLengths.MaxEMail, VspParameter.NotificationURL); }
		}

		[Obsolete("Use BillingPostCode instead", true)]
		public string PostCode
		{
			get { return this[VspParameter.PostCode]; }
			set { SetWithCheckLength(value, StringLengths.MaxSecurityKey, VspParameter.PostCode); }
		}

		public string RelatedSecurityKey
		{
			get { return this[VspParameter.RelatedSecurityKey]; }
			set { SetWithCheckLength(value, StringLengths.MaxSecurityKey, VspParameter.RelatedSecurityKey); }
		}

		public string RelatedTxAuthNo
		{
			get { return this[VspParameter.RelatedTxAuthNo]; }
			set { SetWithCheckLength(value, StringLengths.MaxSecurityKey, VspParameter.RelatedTxAuthNo); }
		}

		public string RelatedVendorTxCode
		{
			get { return this[VspParameter.RelatedVendorTxCode]; }
			set { SetWithCheckLength(value, StringLengths.MaxVendorTxCode, VspParameter.RelatedVendorTxCode); }
		}

		public string RelatedVPSTxID
		{
			get { return this[VspParameter.RelatedVPSTxID]; }
			set { SetWithCheckLength(value, StringLengths.MaxVSPTxID, VspParameter.RelatedVPSTxID); }
		}

		public string SecurityKey
		{
			get { return this[VspParameter.SecurityKey]; }
			set { SetWithCheckLength(value, StringLengths.MaxSecurityKey, VspParameter.SecurityKey); }
		}

		public string StartDate
		{
			get { return this[VspParameter.StartDate]; }
			set
			{
				if (value != null)
				{
					ValidateDate(value);
				}
				this[VspParameter.StartDate] = value;
			}
		}

		public string SuccessURL
		{
			get { return this[VspParameter.SuccessURL]; }
			set { SetRequiredWithCheckLength(value, StringLengths.MaxEMail, VspParameter.SuccessURL); }
		}

		public string ThreeDSecureStatus
		{
			get { return this[VspParameter._3DSecureStatus]; }
			set { SetWithCheckLength(value, StringLengths.MaxIPAddress, VspParameter._3DSecureStatus); }
		}

		public string TxAuthNo
		{
			get { return this[VspParameter.TxAuthNo]; }
			set { SetWithCheckLength(value, StringLengths.MaxSecurityKey, VspParameter.TxAuthNo); }
		}

		public VspTransactionType TxType
		{
			get { return (VspTransactionType) Enum.Parse(typeof (VspTransactionType), this[VspParameter.TxType], true); }
			set { SetWithCheckLength(value.ToString("G").ToUpper(), StringLengths.MaxIPAddress, VspParameter.TxType); }
		}

		public string Vendor
		{
			get { return this[VspParameter.Vendor]; }
			set { SetRequiredWithCheckLength(value, StringLengths.MaxCardHolder, VspParameter.Vendor); }
		}

		public string VendorEMail
		{
			get { return this[VspParameter.VendorEMail]; }
			set { SetWithCheckLength(value, StringLengths.MaxEMail, VspParameter.VendorEMail); }
		}

		public string VendorTxCode
		{
			get { return this[VspParameter.VendorTxCode]; }
			set { SetRequiredWithCheckLength(value, StringLengths.MaxVendorTxCode, VspParameter.VendorTxCode); }
		}

		public string VPSProtocol
		{
			get { return this[VspParameter.VPSProtocol]; }
			set { SetRequiredWithCheckLength(value, StringLengths.MaxDate, VspParameter.VPSProtocol); }
		}

		public string VPSTxID
		{
			get { return this[VspParameter.VPSTxId]; }
			set { SetWithCheckLength(value, StringLengths.MaxVSPTxID, VspParameter.VPSTxId); }
		}

		public string XID
		{
			get { return this[VspParameter.XID]; }
			set { SetWithCheckLength(value, StringLengths.MaxXID, VspParameter.XID); }
		}

		protected internal enum StringLengths
		{
			Max3DSecureStatus = 15,
			MaxAddress = 200,
			MaxAuthCode = 15,
			MaxBasket = 0x1d4c,
			MaxCardHolder = 50,
			MaxCardNumber = 20,
			MaxCardType = 15,
			MaxCAVV = 0x20,
			MaxCurrency = 3,
			MaxCV2 = 4,
			MaxDate = 4,
			MaxDescription = 100,
			MaxECI = 2,
			MaxEMail = 0xff,
			MaxFlag = 1,
			MaxIPAddress = 15,
			MaxIssueNo = 2,
			MaxMessage = 0x1d4c,
			MaxName = 100,
			MaxNumber = 20,
			MaxPostCode = 10,
			MaxProtocol = 4,
			MaxSecurityKey = 10,
			MaxTxAuthNo = 10,
			MaxTxType = 15,
			MaxUrl = 0xff,
			MaxVendor = 50,
			MaxVendorTxCode = 40,
			MaxVSPTxID = 0x26,
			MaxXID = 0x1c
		}
	}
}