using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace Isis.Web.UI.WebControls
{
	public class ConditionalViewItem<TDataItem> : ConditionalViewItem
	{
		public new TDataItem DataItem
		{
			get { return (TDataItem) base.DataItem; }
		}
	}
}
