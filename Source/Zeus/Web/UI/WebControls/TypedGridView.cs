using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zeus.Web.UI.WebControls
{
	/// <summary>
	/// http://www.codeproject.com/KB/aspnet/TypedRepeater.aspx
	/// </summary>
	[ControlBuilder(typeof(TypedGridViewControlBuilder))]
	public class TypedGridView : GridView
	{
		public string DataItemTypeName
		{
			get;
			set;
		}
	}

	public class RealTypedGridView<TDataItem> : TypedGridView
	{
		protected override GridViewRow CreateRow(int rowIndex, int dataSourceIndex, DataControlRowType rowType, DataControlRowState rowState)
		{
			return new TypedGridViewRow<TDataItem>(rowIndex, dataSourceIndex, rowType, rowState);
		}
	}
}