using System;
using System.Collections.Generic;
using System.Linq;
using Isis.ExtensionMethods.Linq;
using Isis.Web.Hosting;
using Zeus.ContentProperties;

[assembly: EmbeddedResourceFile("Zeus.Admin.ReferencingItems.ascx", "Zeus.Admin")]
namespace Zeus.Admin
{
	public partial class ReferencingItems : System.Web.UI.UserControl
	{
		public ContentItem Item { get; set; }

		protected override void OnDataBinding(EventArgs e)
		{
			base.OnDataBinding(e);

			if (Item == null || Item.ID == 0)
				return;

			List<ContentItem> referrers = new List<ContentItem>();
			AddReferencesRecursive(Item, referrers);
			rptItems.DataSource = referrers.Distinct(ci => ci.ID);
		}

		protected void AddReferencesRecursive(ContentItem current, List<ContentItem> referrers)
		{
			referrers.AddRange(Zeus.Context.Finder.Items().Where(ci => ci.DetailsInternal.OfType<LinkProperty>().Any(ld => ld.LinkedItem == current)));
			foreach (ContentItem child in current.GetChildren())
				AddReferencesRecursive(child, referrers);
		}
	}
}