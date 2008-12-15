using System;

namespace Zeus.Persistence
{
	public interface ISelfPersister
	{
		ContentItem CopyTo(ContentItem destination);
		void Delete();
		void MoveTo(ContentItem destination);
		void Save();
	}
}
