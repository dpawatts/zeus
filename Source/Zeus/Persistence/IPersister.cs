using System;

namespace Zeus.Persistence
{
	public interface IPersister
	{
		void Delete(ContentItem contentItem);
		ContentItem Get(int id);
		ContentItem Load(int id);
		void Save(ContentItem contentItem);
	}
}
