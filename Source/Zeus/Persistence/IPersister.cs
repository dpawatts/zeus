using System;

namespace Zeus.Persistence
{
	public interface IPersister
	{
		ContentItem Load(int id);
		void Save(ContentItem contentItem);
	}
}
