using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web.UI;
using System.Drawing.Design;
using System.Web;

namespace Isis.Web.UI.WebControls
{
	[ParseChildren(true, "Items")]
	public class PageUrlParameter : Parameter
	{
		private ListItemCollection _items;
 
		[PersistenceMode(PersistenceMode.InnerDefaultProperty), Editor("System.Web.UI.Design.WebControls.ListItemsCollectionEditor,System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public virtual ListItemCollection Items
		{
			get
			{
				if (_items == null)
				{
					_items = new ListItemCollection();
					if (base.IsTrackingViewState)
						((IStateManager) _items).TrackViewState();
				}
				return this._items;
			}
		}

		protected override object Evaluate(HttpContext context, Control control)
		{
			// Check if current URL matches any of the items - if so, return that value
			string currentUrl = context.Request.Path.ToLower();
			ListItem listItem = this.Items.Cast<ListItem>().SingleOrDefault(li => li.Text.Replace("~", ((HttpRuntime.AppDomainAppVirtualPath == "/") ? string.Empty : HttpRuntime.AppDomainAppVirtualPath)).ToLower() == currentUrl);
			if (listItem != null)
				return listItem.Value;
			else
				return base.Evaluate(context, control);
		}
	}
}
