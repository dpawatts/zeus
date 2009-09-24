using Ninject;
using Zeus.ContentTypes;
using Zeus.Design.Editors;
using Zeus.Templates.Configuration;
using Zeus.Templates.ContentTypes;

namespace Zeus.Templates.Services
{
	public class TaggingDefinitionAppender : IInitializable
	{
		private readonly IContentTypeManager _contentTypeManager;
		private readonly TemplatesSection _templatesConfig;

		public TaggingDefinitionAppender(IContentTypeManager contentTypeManager, TemplatesSection templatesConfig)
		{
			_contentTypeManager = contentTypeManager;
			_templatesConfig = templatesConfig;
		}

		#region IInitializable Members

		public void Initialize()
		{
			if (_templatesConfig.Tagging == null || !_templatesConfig.Tagging.Enabled)
				return;

			foreach (ContentType contentType in _contentTypeManager.GetContentTypes())
			{
				if (IsPage(contentType))
				{
					LinkedItemsCheckBoxListEditorAttribute tagEditable = new LinkedItemsCheckBoxListEditorAttribute();
					tagEditable.Name = "Tags";
					tagEditable.Title = "Tags";
					tagEditable.SortOrder = 500;
					tagEditable.TypeFilter = typeof(Tag);
					tagEditable.ContainerName = "Content";

					contentType.Add(tagEditable);
				}
			}
		}

		private static bool IsPage(ContentType contentType)
		{
			return typeof(BasePage).IsAssignableFrom(contentType.ItemType)
				&& contentType.ItemType != typeof(Redirect)
				&& contentType.ItemType != typeof(Tag)
				&& contentType.ItemType != typeof(TagGroup);
		}

		#endregion
	}
}