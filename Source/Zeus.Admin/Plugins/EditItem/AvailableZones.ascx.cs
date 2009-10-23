using System;
using System.Collections.Generic;
using System.Web.UI;
using Zeus.BaseLibrary.Web;
using Zeus.Web;

namespace Zeus.Admin.Plugins.EditItem
{
	public partial class AvailableZones : System.Web.UI.UserControl
	{
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			this.Visible = this.CurrentItem != null && this.CurrentItem.ID > 0 && this.rptZones.Items.Count > 0;
		}

		public object DataSource
		{
			get { return this.rptZones.DataSource; }
			set { this.rptZones.DataSource = value; }
		}

		public ContentItem CurrentItem { get; set; }

		protected string GetNewDataItemText(object dataItem)
		{
			return DataBinder.Eval(dataItem, "Title").ToString();
		}

		protected string GetNewDataItemUrl(object dataItem)
		{
			Integrity.AvailableZoneAttribute a = (Integrity.AvailableZoneAttribute) dataItem;
			return Zeus.Context.Current.AdminManager.GetSelectNewItemUrl(CurrentItem, a.ZoneName);
		}

		protected string GetEditDataItemText(object dataItem)
		{
			ContentItem item = (ContentItem) dataItem;
			return string.Format("<img src='{0}'>{1}", Url.ToAbsolute(item.IconUrl), string.IsNullOrEmpty(item.Title) ? "(untitled)" : item.Title);
		}

		protected string GetEditDataItemUrl(object dataItem)
		{
			return Zeus.Context.Current.AdminManager.GetEditExistingItemUrl((ContentItem) dataItem);
		}

		protected IEnumerable<WidgetContentItem> GetItemsInZone(object dataItem)
		{
			Integrity.AvailableZoneAttribute a = (Integrity.AvailableZoneAttribute) dataItem;
			return Zeus.Context.Current.ContentManager.GetWidgets(CurrentItem, a.ZoneName);
		}

		protected string GetZoneString(string key)
		{
			return key;
			//return Utility.GetGlobalResourceString("Zones", key);
		}
	}
}