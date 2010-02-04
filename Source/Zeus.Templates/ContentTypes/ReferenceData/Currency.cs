using Ext.Net;
using Zeus.Integrity;
using Zeus.Design.Editors;

namespace Zeus.Templates.ContentTypes.ReferenceData
{
	[ContentType("Currency")]
	[RestrictParents(typeof(CurrencyList))]
	public class Currency : BaseContentItem
	{
		public Currency()
		{

		}

		public Currency(string title, string isoCode, string symbol)
		{
			Title = title;
			IsoCode = isoCode;
			Symbol = symbol;

			Icon icon = Icon.MoneyAdd;
			switch (isoCode)
			{
				case "GBP" :
					icon = Icon.MoneyPound;
					break;
				case "USD":
					icon = Icon.MoneyDollar;
					break;
				case "EUR":
					icon = Icon.MoneyEuro;
					break;
				case "JPY":
					icon = Icon.MoneyYen;
					break;
			}
			CurrencyIcon = icon;
		}

		[TextBoxEditor("Name", 10, Required = true)]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}

		[ContentProperty("ISO Code", 100)]
		public string IsoCode
		{
			get { return GetDetail("IsoCode", string.Empty); }
			set { SetDetail("IsoCode", value); }
		}

		[ContentProperty("Symbol", 110)]
		public string Symbol
		{
			get { return GetDetail("Symbol", string.Empty); }
			set { SetDetail("Symbol", value); }
		}

		[ContentProperty("Exchange Rate", 120)]
		public decimal ExchangeRate
		{
			get { return GetDetail("ExchangeRate", 1.0m); }
			set { SetDetail("ExchangeRate", value); }
		}

		public Icon CurrencyIcon
		{
			get { return GetDetail("CurrencyIcon", Icon.MoneyAdd); }
			set { SetDetail("CurrencyIcon", value); }
		}

		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(CurrencyIcon); }
		}
	}
}
