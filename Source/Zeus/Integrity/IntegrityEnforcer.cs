using Castle.Core;
using Zeus.Persistence;

namespace Zeus.Integrity
{
	public class IntegrityEnforcer : IIntegrityEnforcer, IStartable
	{
		private readonly IPersister _persister;
		private readonly IIntegrityManager _integrityManager;

		public IntegrityEnforcer(IPersister persister, IIntegrityManager integrityManager)
		{
			_persister = persister;
			_integrityManager = integrityManager;
		}

		#region Event Dispatchers

		private void ItemSavingEvenHandler(object sender, CancelItemEventArgs e)
		{
			OnItemSaving(e.AffectedItem);
		}

		private void ItemMovingEvenHandler(object sender, CancelDestinationEventArgs e)
		{
			OnItemMoving(e.AffectedItem, e.Destination);
		}

		private void ItemDeletingEvenHandler(object sender, CancelItemEventArgs e)
		{
			OnItemDeleting(e.AffectedItem);
		}

		private void ItemCopyingEvenHandler(object sender, CancelDestinationEventArgs e)
		{
			OnItemCopying(e.AffectedItem, e.Destination);
		}

		#endregion

		#region Event Handlers

		protected virtual void OnItemCopying(ContentItem source, ContentItem destination)
		{
			ZeusException ex = _integrityManager.GetCopyException(source, destination);
			if (ex != null)
				throw ex;
		}

		protected virtual void OnItemDeleting(ContentItem item)
		{
			ZeusException ex = _integrityManager.GetDeleteException(item);
			if (ex != null)
				throw ex;
		}

		protected virtual void OnItemMoving(ContentItem source, ContentItem destination)
		{
			ZeusException ex = _integrityManager.GetMoveException(source, destination);
			if (ex != null)
				throw ex;
		}

		protected virtual void OnItemSaving(ContentItem item)
		{
			ZeusException ex = _integrityManager.GetSaveException(item);
			if (ex != null)
				throw ex;
		}

		#endregion

		#region IStartable Members

		public virtual void Start()
		{
			_persister.ItemCopying += ItemCopyingEvenHandler;
			_persister.ItemDeleting += ItemDeletingEvenHandler;
			_persister.ItemMoving += ItemMovingEvenHandler;
			_persister.ItemSaving += ItemSavingEvenHandler;
		}

		public virtual void Stop()
		{
			_persister.ItemCopying -= ItemCopyingEvenHandler;
			_persister.ItemDeleting -= ItemDeletingEvenHandler;
			_persister.ItemMoving -= ItemMovingEvenHandler;
			_persister.ItemSaving -= ItemSavingEvenHandler;
		}

		#endregion
	}
}
