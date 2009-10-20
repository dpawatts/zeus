using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Isis.Web.UI.WebControls
{
	/// <summary>
	/// http://www.codeproject.com/KB/aspnet/TypedRepeater.aspx
	/// </summary>
	[ControlBuilder(typeof(TypedRepeaterControlBuilder))]
	public class TypedRepeater : System.Web.UI.WebControls.Repeater
	{
		public string DataItemTypeName
		{
			get;
			set;
		}
	}

	public class TypedRepeater<TDataItem> : TypedRepeater
	{
		protected override RepeaterItem CreateItem(int itemIndex, ListItemType itemType)
		{
			return new TypedRepeaterItem<TDataItem>(itemIndex, itemType);
		}
	}
}
