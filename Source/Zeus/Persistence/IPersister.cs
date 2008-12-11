using System;

namespace Zeus.Persistence
{
	public interface IPersister
	{
		void Delete(ContentItem contentItem);
		ContentItem Get(int id);
		ContentItem Load(int id);
		void Move(ContentItem toMove, ContentItem newParent);
		void Save(ContentItem contentItem);
		void UpdateSortOrder(ContentItem contentItem, int newPos);
	}
}
