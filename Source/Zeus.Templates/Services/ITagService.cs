using Zeus.Templates.ContentTypes;

namespace Zeus.Templates.Services
{
	public interface ITagService
	{
		Tag EnsureTag(ContentItem currentItem, string tagName);
	}
}