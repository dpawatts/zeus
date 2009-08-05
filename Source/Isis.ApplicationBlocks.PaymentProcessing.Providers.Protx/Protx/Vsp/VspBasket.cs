using System;
using System.Collections;
using System.Text;

namespace Protx.Vsp
{
	public class VspBasket : CollectionBase
	{
		public void Add(IVspBasketItem item)
		{
			base.List.Add(item);
		}

		private static void AppendField(StringBuilder sb, string data)
		{
			if (data != null)
			{
				sb.Append(data);
			}
		}

		public void Remove(int index)
		{
			if ((index > (base.List.Count - 1)) || (index < 0))
			{
				throw new ArgumentOutOfRangeException("index", index, "Index not valid!");
			}
			base.List.RemoveAt(index);
		}

		public override string ToString()
		{
			if (base.List.Count == 0)
			{
				return null;
			}
			StringBuilder sb = new StringBuilder();
			sb.Append(base.List.Count);
			foreach (IVspBasketItem item in base.List)
			{
				sb.Append(':');
				AppendField(sb, item.Description.Replace(":", "#"));
				sb.Append(':');
				AppendField(sb, item.Quantity);
				sb.Append(':');
				AppendField(sb, item.ItemPrice);
				sb.Append(':');
				AppendField(sb, item.ItemTax);
				sb.Append(':');
				AppendField(sb, item.ItemTotal);
				sb.Append(':');
				AppendField(sb, item.LineTotal);
			}
			return sb.ToString();
		}

		public IVspBasketItem this[int index]
		{
			get { return (base.List[index] as IVspBasketItem); }
			set { base.List[index] = value; }
		}
	}
}