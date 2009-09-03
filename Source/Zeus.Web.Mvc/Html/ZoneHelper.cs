using System.Collections.Generic;
using System.Text;
using System.Web.Routing;
using Zeus.Web.UI;

namespace Zeus.Web.Mvc.Html
{
	public class ZoneHelper : ItemHelper
	{
		private readonly ITemplateRenderer _templateRenderer = Context.Current.Resolve<ITemplateRenderer>();

		public ZoneHelper(IContentItemContainer container, string zoneName)
			: base(container)
		{
			ZoneName = zoneName;
		}

		public ZoneHelper(IContentItemContainer container, string zoneName, ContentItem item)
			: base(container, item)
		{
			ZoneName = zoneName;
		}

		protected System.Web.Mvc.TagBuilder TagBuilder { get; set; }

		protected string ZoneName { get; set; }

		public ZoneHelper WrapIn(string tagName, object attributes)
		{
			TagBuilder = new System.Web.Mvc.TagBuilder(tagName);
			TagBuilder.MergeAttributes(new RouteValueDictionary(attributes));

			return this;
		}

		public override string ToString()
		{
			var partialResult = new StringBuilder();

			foreach (var child in GetItems())
			{
				ContentItem model = child;
				string partial = _templateRenderer.RenderTemplate(model, Container);

				if (TagBuilder == null)
				{
					partialResult.AppendLine(partial);
					continue;
				}
				TagBuilder.InnerHtml = partial;
				partialResult.AppendLine(TagBuilder.ToString());
			}

			return partialResult.ToString();
		}

		private IEnumerable<ContentItem> GetItems()
		{
			if (PartsAdapter == null)
				return CurrentItem.GetChildren(ZoneName);

			return PartsAdapter.GetItemsInZone(CurrentItem, ZoneName);
		}
	}
}