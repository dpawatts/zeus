using System.Configuration;
using MbUnit.Framework;
using Zeus.Configuration;
using Zeus.DynamicContent;

namespace Zeus.Tests.DynamicContent
{
	[TestFixture]
	public class DynamicContentManagerTests
	{
		[Test]
		public void CanRenderDynamicContent()
		{
			DynamicContentSection configSection = ConfigurationManager.GetSection("zeus/dynamicContent") as DynamicContentSection;
			DynamicContentManager manager = new DynamicContentManager(configSection);
			const string testString = @"Hello blah
				<span class=""mceNonEditable"" state=""3,MyPropName"">{DynamicContent:DynamicPageProperty}</span>
				Some more text";
			string result = manager.RenderDynamicContent(testString);
		}
	}
}