using System.Linq;
using Zeus.Templates.ContentTypes.ReferenceData;

namespace Zeus.Templates.Services
{
	public class CurrencyService : ICurrencyService
	{
		public decimal Convert(string toIsoCode, decimal amount)
		{
			CurrencyList currencyList = (CurrencyList) Find.RootItem.GetChild("system").GetChild("reference-data").GetChild("currencies");

			Currency baseCurrency = currencyList.BaseCurrency;
			Currency toCurrency = currencyList.GetChildren<Currency>().Single(c => c.IsoCode == toIsoCode);

			if (baseCurrency == toCurrency)
				return amount;
			return toCurrency.ExchangeRate * amount;
		}
	}
}