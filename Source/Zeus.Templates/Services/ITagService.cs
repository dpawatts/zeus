using System.Collections.Generic;
using Zeus.Templates.ContentTypes;

namespace Zeus.Templates.Services
{
	public interface ITagService
	{
		Tag EnsureTag(TagGroup tagGroup, string tagName);
		IEnumerable<Tag> GetActiveTags(TagGroup tagGroup);
		TagGroup GetCurrentTagGroup(ContentItem currentItem);
		int GetReferenceCount(Tag tag);
		IEnumerable<ContentItem> GetTaggedItems(Tag tag);
	}
}