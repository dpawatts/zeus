namespace Protx.Vsp
{
	public interface IVspBasketItem
	{
		string Description { get; }

		string ItemPrice { get; }

		string ItemTax { get; }

		string ItemTotal { get; }

		string LineTotal { get; }

		string Quantity { get; }
	}
}