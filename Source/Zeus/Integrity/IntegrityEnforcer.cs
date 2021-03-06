﻿using Ninject;
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

		private void ItemSavingEventHandler(object sender, CancelItemEventArgs e)
		{
			OnItemSaving(e.AffectedItem);
		}

		private void ItemMovingEventHandler(object sender, CancelDestinationEventArgs e)
		{
			OnItemMoving(e.AffectedItem, e.Destination);
		}

		private void ItemDeletingEventHandler(object sender, CancelItemEventArgs e)
		{
			OnItemDeleting(e.AffectedItem);
		}

		private void ItemCopyingEventHandler(object sender, CancelDestinationEventArgs e)
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
			_persister.ItemCopying += ItemCopyingEventHandler;
			_persister.ItemDeleting += ItemDeletingEventHandler;
			_persister.ItemMoving += ItemMovingEventHandler;
			_persister.ItemSaving += ItemSavingEventHandler;
		}

		public virtual void Stop()
		{
			_persister.ItemCopying -= ItemCopyingEventHandler;
			_persister.ItemDeleting -= ItemDeletingEventHandler;
			_persister.ItemMoving -= ItemMovingEventHandler;
			_persister.ItemSaving -= ItemSavingEventHandler;
		}

		#endregion
	}
}
