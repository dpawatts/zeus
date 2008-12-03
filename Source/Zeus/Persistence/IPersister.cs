using System;

namespace Zeus.Persistence
{
	public interface IPersister
	{
		void Save(ContentItem contentItem);
	}
}
