using System;
using System.Web.UI.WebControls;

namespace Isis.Web.UI.WebControls
{
	/// <summary>
	/// http://www.codeproject.com/KB/aspnet/TypedRepeater.aspx
	/// </summary>
	/// <typeparam name="TDataItem"></typeparam>
	public class TypedRepeaterItem<TDataItem> : System.Web.UI.WebControls.RepeaterItem
	{
		public TypedRepeaterItem(int itemIndex, ListItemType itemType)
			: base(itemIndex, itemType)
		{
		}

		public new TDataItem DataItem
		{
			get { return (TDataItem) base.DataItem; }
			set { base.DataItem = (TDataItem) value; }
		}
	}
}
