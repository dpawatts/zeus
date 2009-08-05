using System;

namespace Protx.Vsp
{
	public class VspBasketItem : IVspBasketItem
	{
		private string _description;
		private VspBasketItemFields _fields;
		private decimal _itemPrice;
		private decimal _itemTax;
		private decimal _itemTotal;
		private decimal _lineTotal;
		private readonly IFormatProvider _provider;
		private short _quantity;

		public VspBasketItem()
		{
			_provider = null;
		}

		public VspBasketItem(IFormatProvider provider)
		{
			_provider = provider;
		}

		protected bool IsFieldSet(VspBasketItemFields field)
		{
			return ((_fields & field) != 0);
		}

		public string Description
		{
			get { return _description; }
			set
			{
				_description = value;
				_fields |= VspBasketItemFields.Description;
			}
		}

		public decimal ItemPrice
		{
			get { return _itemPrice; }
			set
			{
				_itemPrice = value;
				_fields |= VspBasketItemFields.ItemPrice;
			}
		}

		public decimal ItemTax
		{
			get { return _itemTax; }
			set
			{
				_itemTax = value;
				_fields |= VspBasketItemFields.ItemTax;
			}
		}

		public decimal ItemTotal
		{
			get { return _itemTotal; }
			set
			{
				_itemTotal = value;
				_fields |= VspBasketItemFields.ItemTotal;
			}
		}

		public decimal LineTotal
		{
			get { return _lineTotal; }
			set
			{
				_lineTotal = value;
				_fields |= VspBasketItemFields.LineTotal;
			}
		}

		string IVspBasketItem.Description
		{
			get { return _description; }
		}

		string IVspBasketItem.ItemPrice
		{
			get
			{
				if (!IsFieldSet(VspBasketItemFields.ItemPrice))
				{
					return null;
				}
				return _itemPrice.ToString("C", _provider);
			}
		}

		string IVspBasketItem.ItemTax
		{
			get
			{
				if (!IsFieldSet(VspBasketItemFields.ItemTax))
				{
					return null;
				}
				return _itemTax.ToString("C", _provider);
			}
		}

		string IVspBasketItem.ItemTotal
		{
			get
			{
				if (!IsFieldSet(VspBasketItemFields.ItemTotal))
				{
					return null;
				}
				return _itemTotal.ToString("C", _provider);
			}
		}

		string IVspBasketItem.LineTotal
		{
			get
			{
				if (!IsFieldSet(VspBasketItemFields.LineTotal))
				{
					return null;
				}
				return _lineTotal.ToString("C", _provider);
			}
		}

		string IVspBasketItem.Quantity
		{
			get
			{
				if (!IsFieldSet(VspBasketItemFields.Quantity))
				{
					return null;
				}
				return _quantity.ToString(_provider);
			}
		}

		public short Quantity
		{
			get { return _quantity; }
			set
			{
				_quantity = value;
				_fields |= VspBasketItemFields.Quantity;
			}
		}

		[Flags]
		protected enum VspBasketItemFields
		{
			Description = 1,
			ItemPrice = 4,
			ItemTax = 8,
			ItemTotal = 0x10,
			LineTotal = 0x20,
			Quantity = 2
		}
	}
}