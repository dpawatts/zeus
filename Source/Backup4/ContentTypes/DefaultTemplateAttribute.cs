using Zeus.Web;

namespace Zeus.Templates.ContentTypes
{
	public class DefaultTemplateAttribute : TemplateAttribute
	{
		public const string DefaultTemplateFolderPath = "~/UI/Views/";

		public DefaultTemplateAttribute()
		{
		}

		public DefaultTemplateAttribute(string viewName)
			: base(DefaultTemplateFolderPath + viewName + ".aspx")
		{
		}

		protected override string GetTemplateUrl(ContentItem item)
		{
			return DefaultTemplateFolderPath + item.GetType().Name + ".aspx";
		}
	}
}