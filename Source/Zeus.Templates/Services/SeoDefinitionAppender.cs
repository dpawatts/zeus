using Ninject;
using Zeus.ContentTypes;
using Zeus.Design.Editors;
using Zeus.Templates.Configuration;
using Zeus.Templates.ContentTypes;
using Zeus.Web.UI;

namespace Zeus.Templates.Services
{
	public class SeoDefinitionAppender : IInitializable
	{
		private readonly IContentTypeManager _contentTypeManager;
		private readonly TemplatesSection _templatesConfig;

		public SeoDefinitionAppender(IContentTypeManager contentTypeManager, TemplatesSection templatesConfig)
		{
			_contentTypeManager = contentTypeManager;
			_templatesConfig = templatesConfig;

			HtmlTitleTitle = "HTML Title";
			MetaKeywordsTitle = "Meta Keywords";
			MetaDescriptionTitle = "Meta Description";
			SeoTabTitle = "SEO";
		}

		public string HtmlTitleTitle { get; set; }
		public string MetaKeywordsTitle { get; set; }
		public string MetaDescriptionTitle { get; set; }
		public string SeoTabTitle { get; set; }

		#region IInitializable Members

		public void Initialize()
		{
			if (_templatesConfig.Seo == null || !_templatesConfig.Seo.Enabled)
				return;

			foreach (ContentType contentType in _contentTypeManager.GetContentTypes())
			{
				if (IsPage(contentType))
				{
					TabPanelAttribute seoTab = new TabPanelAttribute("SEO", SeoTabTitle, 20);
					contentType.Add(seoTab);

					AddEditableText(contentType, HtmlTitleTitle, SeoUtility.HTML_TITLE, 11, _templatesConfig.Seo.HtmlTitleFormat, "Used in the &lt;title&gt; element on the page", 200, false);
					AddEditableText(contentType, MetaKeywordsTitle, SeoUtility.META_KEYWORDS, 21, _templatesConfig.Seo.MetaKeywordsFormat, null, 400, false);
					AddEditableText(contentType, MetaDescriptionTitle, SeoUtility.META_DESCRIPTION, 22, _templatesConfig.Seo.MetaDescriptionFormat, null, 1000, true);
				}
			}
		}

		private static void AddEditableText(ContentType contentType, string title, string name, int sortOrder, string formatString, string description, int maxLength, bool multiline)
		{
			ReactiveTextBoxEditorAttribute editor = new ReactiveTextBoxEditorAttribute(title, sortOrder, formatString)
			{
				Name = name,
				ContainerName = "SEO",
				MaxLength = maxLength,
				Description = description,
				Shared = false,
				PropertyType = typeof(string)
			};
			if (multiline)
				editor.TextMode = System.Web.UI.WebControls.TextBoxMode.MultiLine;
			contentType.Add(editor);
			contentType.AddProperty(new ContentPropertyAttribute(typeof(string), title, sortOrder) { Name = name, Description = description, EditorContainerName = "SEO", Shared = false });
		}

		private static bool IsPage(ContentType contentType)
		{
			return typeof(BasePage).IsAssignableFrom(contentType.ItemType)
				&& contentType.ItemType != typeof(Redirect)
				&& contentType.IsPage;
		}

		#endregion
	}
}