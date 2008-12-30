using System;
using System.Linq;
using Zeus.ContentTypes.Properties;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Zeus;

namespace Bermedia.Gibbons.Web.Items.Details
{
	public class ProductColoursEditor : CheckBoxListEditorAttribute
	{
		public ProductColoursEditor(string title, int sortOrder)
			: base(title, sortOrder)
		{
		}

		protected override IEnumerable GetSelectedItems(IEnumerable<ListItem> selectedListItems)
		{
			List<ContentItem> products = new List<ContentItem>();
			foreach (ListItem listItem in selectedListItems)
				products.Add(Zeus.Context.Persister.Get(Convert.ToInt32(listItem.Value)));
			return products;
		}

		protected override string GetValue(object detail)
		{
			return ((ProductColour) detail).ID.ToString();
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
