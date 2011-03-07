namespace Zeus.Web.UI.WebControls
{
	/// <summary>
	/// http://www.codeproject.com/KB/aspnet/TypedRepeater.aspx
	/// </summary>
	/// <typeparam name="TDataItem"></typeparam>
	public class TypedListViewDataItem<TDataItem> : System.Web.UI.WebControls.ListViewDataItem
	{
		public TypedListViewDataItem(int dataItemIndex, int displayIndex)
			: base(dataItemIndex, displayIndex)
		{
		}

		public new TDataItem DataItem
		{
			get { return (TDataItem) base.DataItem; }
			set { base.DataItem = (TDataItem) value; }
		}
	}
}