using System;
using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.Design.Editors;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.ECommerce.ContentTypes.Data
{
	[ContentType]
	[RestrictParents(typeof(ShoppingBasket))]
	public class PaymentCard : BaseContentItem
	{
		public override string IconUrl
		{
			get { return GetIconUrl(typeof(Shop), "Zeus.AddIns.ECommerce.Icons.visa.png"); }
		}

		[ContentProperty("CardType", 200)]
		public string CardType
		{
			get { return GetDetail("CardType", string.Empty); }
			set { SetDetail("CardType", value); }
		}

		[ContentProperty("Name On Card", 210)]
		public string NameOnCard
		{
			get { return GetDetail("NameOnCard", string.Empty); }
			set { SetDetail("NameOnCard", value); }
		}

		[ContentProperty("MaskedCardNumber", 220)]
		public string MaskedCardNumber
		{
			get { return GetDetail("MaskedCardNumber", string.Empty); }
			set { SetDetail("MaskedCardNumber", value); }
		}

		[ContentProperty("Expiry Month", 230)]
		public int ExpiryMonth
		{
			get { return GetDetail("ExpiryMonth", 0); }
			set { SetDetail("ExpiryMonth", value); }
		}

		[ContentProperty("Expiry Year", 240)]
		public int ExpiryYear
		{
			get { return GetDetail("ExpiryYear", 0); }
			set { SetDetail("ExpiryYear", value); }
		}

		public DateTime ValidTo
		{
			get
			{
				// Sets the date to the last day of the month.
				return new DateTime(ExpiryYear, ExpiryMonth, DateTime.DaysInMonth(ExpiryYear, ExpiryMonth));
			}
		}

		[ContentProperty("Start Month", 250)]
		public int? StartMonth
		{
			get { return GetDetail<int?>("StartMonth", null); }
			set { SetDetail("StartMonth", value); }
		}

		[ContentProperty("Start Year", 260)]
		public int? StartYear
		{
			get { return GetDetail<int?>("StartYear", null); }
			set { SetDetail("StartYear", value); }
		}

		public DateTime? ValidFrom
		{
			get
			{
				if (StartMonth != null && StartYear != null)
					// Sets the date to the first day of the month.
					return new DateTime(StartYear.Value, StartMonth.Value, 1);
				return null;
			}
		}

		[ContentProperty("Issue Number", 270)]
		public string IssueNumber
		{
			get { return GetDetail("IssueNumber", string.Empty); }
			set { SetDetail("IssueNumber", value); }
		}
	}
}