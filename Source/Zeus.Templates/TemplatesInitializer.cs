using System.Configuration;
using Zeus.Engine;
using Zeus.Plugin;
using Zeus.Templates.Configuration;
using Zeus.Templates.Services;

namespace Zeus.Templates
{
	[AutoInitialize]
	public class TemplatesInitializer : IPluginInitializer
	{
		public void Initialize(ContentEngine engine)
		{
			engine.AddComponent("zeus.templates.services.news", typeof(NewsService));

			TemplatesSection configSection = ConfigurationManager.GetSection("zeus/templates") as TemplatesSection;
			if (configSection != null)
			{
				engine.AddComponentInstance("zeus.templates.configuration.templatesection", configSection);
				engine.AddComponent("zeus.templates.services.configuration", typeof(ConfigurationService));
			}
		}
	}
}