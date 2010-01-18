using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using Zeus.Web.UI;

namespace Zeus.Web.Mvc.Html
{
	public abstract class BaseWidgetHelper : ItemHelper
	{
		private readonly ITemplateRenderer _templateRenderer = Context.Current.Resolve<ITemplateRenderer>();

		protected BaseWidgetHelper(HtmlHelper htmlHelper, IContentItemContainer container, string actionName)
			: base(htmlHelper, container)
		{
			ActionName = actionName;
		}

		protected BaseWidgetHelper(HtmlHelper htmlHelper, IContentItemContainer container, ContentItem item, string actionName)
			: base(htmlHelper, container, item)
		{
			ActionName = actionName;
		}

		protected System.Web.Mvc.TagBuilder TagBuilder { get; set; }

		protected string ActionName { get; set; }

		public BaseWidgetHelper WrapIn(string tagName, object attributes)
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
				string partial = _templateRenderer.RenderTemplate(HtmlHelper, model, Container, ActionName);

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

		protected abstract IEnumerable<WidgetContentItem> GetItems();
	}
}