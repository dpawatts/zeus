using System.Web.UI.WebControls;

namespace Isis.Web.UI.WebControls
{
	/// <summary>
	/// http://www.codeproject.com/KB/aspnet/TypedRepeater.aspx
	/// </summary>
	/// <typeparam name="TDataItem"></typeparam>
	public class TypedGridViewRow<TDataItem> : GridViewRow
	{
		public TypedGridViewRow(int rowIndex, int dataItemIndex, DataControlRowType rowType, DataControlRowState rowState)
			: base(rowIndex, dataItemIndex, rowType, rowState)
		{
		}

		public new TDataItem DataItem
		{
			get { return (TDataItem) base.DataItem; }
			set { base.DataItem = value; }
		}
	}
}