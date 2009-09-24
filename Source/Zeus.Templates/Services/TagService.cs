using System;
using System.Collections.Generic;
using System.Linq;
using Zeus.ContentProperties;
using Zeus.ContentTypes;
using Zeus.Persistence;
using Zeus.Templates.ContentTypes;

namespace Zeus.Templates.Services
{
	public class TagService : ITagService
	{
		private readonly IFinder<LinkProperty> _tagFinder;
		private readonly IContentTypeManager _contentTypeManager;
		private readonly IPersister _persister;

		public TagService(IFinder<LinkProperty> tagFinder, IContentTypeManager contentTypeManager, IPersister persister)
		{
			_tagFinder = tagFinder;
			_contentTypeManager = contentTypeManager;
			_persister = persister;
		}

		public Tag EnsureTag(TagGroup tagGroup, string tagName)
		{
			foreach (Tag childTag in tagGroup.GetChildren<Tag>())
				if (childTag.Title == tagName)
					return childTag;

			Tag tag = _contentTypeManager.CreateInstance<Tag>(tagGroup);
			tag.Name = Utility.GetSafeName(tagName);
			tag.Title = tagName;
			_persister.Save(tag);

			return tag;
		}

		public IEnumerable<Tag> GetActiveTags(TagGroup tagGroup)
		{
			return tagGroup.GetChildren<Tag>().Where(t => GetTaggedItems(t).Any())
				.OrderBy(t => t.Title);
		}

		public TagGroup GetCurrentTagGroup(ContentItem currentItem)
		{
			// Look up in the tree from the current item until we find an item with a
			// TagGroup child.
			foreach (ContentItem ancestor in Find.EnumerateParents(currentItem, null, true))
			{
				IEnumerable<TagGroup> tagGroups = ancestor.GetChildren<TagGroup>();
				if (tagGroups.Any())
					return tagGroups.First();
			}
			return null;
		}

		public int GetReferenceCount(Tag tag)
		{
			return GetTaggedItems(tag).Count();
		}

		public IEnumerable<ContentItem> GetTaggedItems(Tag tag)
		{
			return _tagFinder.Items()
				.Where(lp => lp.LinkedItem == tag)
				.ToList()
				.Select(lp => lp.EnclosingItem)
				.Distinct();
		}
	}
}