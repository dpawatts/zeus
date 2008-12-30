using System;
using System.Linq;
using Zeus.ContentTypes.Properties;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Zeus;

namespace Bermedia.Gibbons.Web.Items.Details
{
	public class ProductScentsEditor : ProductColoursEditor
	{
		public ProductScentsEditor(string title, int sortOrder)
			: base(title, sortOrder)
		{
		}

		protected override ListItem[] GetListItems(ContentItem item)
		{
			return Zeus.Context.Current.Finder.Elements<ProductScent>()
				.ToList()
				.Select(p => new ListItem(p.Title, p.ID.ToString()))
				.ToArray();
		}
	}
}
