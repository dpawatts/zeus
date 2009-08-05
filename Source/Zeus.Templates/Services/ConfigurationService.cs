using System.Linq;
using Ninject;
using Zeus.ContentTypes;
using Zeus.Templates.Configuration;

namespace Zeus.Templates.Services
{
	public class ConfigurationService : IInitializable
	{
		#region Fields

		private readonly TemplatesSection _configSection;
		private readonly IContentTypeManager _contentTypeManager;

		#endregion

		#region Constructor

		public ConfigurationService(TemplatesSection configSection, IContentTypeManager contentTypeManager)
		{
			_configSection = configSection;
			_contentTypeManager = contentTypeManager;
		}

		#endregion

		#region Methods

		public void Initialize()
		{
			// For each registered content types, apply filters specified in <templates> config section.
			foreach (ContentType contentType in _contentTypeManager.GetContentTypes().Where(ct => ct.ItemType.FullName.StartsWith("Zeus.Templates")))
				if (!_configSection.Rules.IsContentTypeAllowed(contentType))
					contentType.Enabled = false;
		}

		#endregion
	}
}