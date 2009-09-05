using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.Admin;

namespace Zeus.AddIns.Blogs.Plugins
{
	public class ImportExportBlogXmlActionPluginAttribute : ActionPluginAttribute
	{
		public ImportExportBlogXmlActionPluginAttribute(string name, string text, int sortOrder, string pageResourcePath)
			: base(name, text, null, "ImportExportBlogXml", sortOrder, null, pageResourcePath, "selected={selected}", Targets.Preview, "Zeus.AddIns.Blogs.Icons.package_come_and_go.png")
		{
			TypeFilter = typeof(Blog);
		}
	}
}