using System;
using NHibernate;

namespace Zeus.Persistence.NH
{
	public class Transaction : ITransaction
	{
		private readonly NHibernate.ITransaction transaction;
		private readonly bool isOriginator = true;

		public Transaction(ISessionProvider sessionProvider)
		{
			ISession session = sessionProvider.OpenSession.Session;
			transaction = session.Transaction;
			if (transaction.IsActive)
				isOriginator = false; // The method that first opened the transaction should also close it
			else
				transaction.Begin();
		}

		#region ITransaction Members

		public void Commit()
		{
			if (isOriginator && !transaction.WasCommitted && !transaction.WasRolledBack)
				transaction.Commit();
		}

		public void Rollback()
		{
			if (!transaction.WasCommitted && !transaction.WasRolledBack)
				transaction.Rollback();
		}

		#endregion

		#region IDisposable Members

		void IDisposable.Dispose()
		{
			if (isOriginator)
			{
				Rollback();
				transaction.Dispose();
			}
		}

		#endregion
	}
}
