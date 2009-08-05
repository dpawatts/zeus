using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace Isis.Web.UI.WebControls
{
	/// <summary>
	/// http://www.codeproject.com/KB/aspnet/TypedRepeater.aspx
	/// </summary>
	[ControlBuilder(typeof(TypedExtendedFormViewControlBuilder))]
	public class TypedExtendedFormView : ExtendedFormView
	{
		public string DataItemTypeName
		{
			get;
			set;
		}
	}

	public class TypedExtendedFormView<TDataItem> : TypedExtendedFormView
	{
		public new TDataItem DataItem
		{
			get { return (TDataItem) base.DataItem; }
		}
	}
}
