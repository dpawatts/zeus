namespace Zeus.Templates.Services
{
	public interface ICurrencyService
	{
		decimal Convert(string toIsoCode, decimal amount);
	}
}