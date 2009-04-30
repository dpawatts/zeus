using System;

namespace Zeus.Persistence
{
	public interface ITransaction : IDisposable
	{
		void Commit();
		void Rollback();
	}
}
