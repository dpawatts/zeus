using Ninject;
using Zeus.Configuration;

namespace Zeus.ContentTypes
{
	public class ContentTypeConfigurationService : IInitializable
	{
		#region Fields

		private readonly ContentTypesSection _configSection;
		private readonly IContentTypeManager _contentTypeManager;

		#endregion

		#region Constructor

		public ContentTypeConfigurationService(ContentTypesSection configSection, IContentTypeManager contentTypeManager)
		{
			_configSection = configSection;
			_contentTypeManager = contentTypeManager;
		}

		#endregion

		#region Methods

		public void Initialize()
		{
			// For each registered content types, apply filters specified in <contentTypes> config section.
			foreach (ContentType contentType in _contentTypeManager.GetContentTypes())
				if (!_configSection.Rules.IsContentTypeAllowed(contentType))
					contentType.Enabled = false;

			// Add configured zones to page content types.
			foreach (ContentTypeSettingsElement settings in _configSection.Settings)
			{
				var contentTypes = (settings.AllSpecified) ? _contentTypeManager.GetContentTypes() : new[] { _contentTypeManager.GetContentType(settings.GetSystemType()) };
				foreach (ContentType contentType in contentTypes) //.Where(ct => ct.IsPage))
					foreach (ContentTypeZone templateZone in settings.Zones)
						contentType.AddAvailableZone(templateZone.Name, templateZone.Title);
			}
		}

		#endregion
	}
}