using System;

namespace Zeus.Persistence
{
	public interface IPersister
	{
		ContentItem Get(int id);
		ContentItem Load(int id);
		void Save(ContentItem contentItem);
	}
}
