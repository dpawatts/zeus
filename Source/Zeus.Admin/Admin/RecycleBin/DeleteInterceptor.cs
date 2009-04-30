using Castle.Core;
using Zeus.Persistence;

namespace Zeus.Admin.RecycleBin
{
	/// <summary>
	/// Intercepts delete operations.
	/// </summary>
	public class DeleteInterceptor : IStartable
	{
		private readonly IPersister _persister;
		private readonly IRecycleBinHandler _recycleBinHandler;

		public DeleteInterceptor(IPersister persister, IRecycleBinHandler recycleBinHandler)
		{
			_persister = persister;
			_recycleBinHandler = recycleBinHandler;
		}

		public void Start()
		{
			_persister.ItemDeleting += OnItemDeleting;
			_persister.ItemMoving += OnItemMoved;
			_persister.ItemCopied += OnItemCopied;
		}

		public void Stop()
		{
			_persister.ItemDeleting -= OnItemDeleting;
			_persister.ItemMoving -= OnItemMoved;
			_persister.ItemCopied -= OnItemCopied;
		}

		private void OnItemCopied(object sender, DestinationEventArgs e)
		{
			if (LeavingTrash(e))
				_recycleBinHandler.RestoreValues(e.AffectedItem);
			else if (_recycleBinHandler.IsInTrash(e.Destination))
				_recycleBinHandler.ExpireTrashedItem(e.AffectedItem);
		}

		private void OnItemMoved(object sender, CancelDestinationEventArgs e)
		{
			if (LeavingTrash(e))
				_recycleBinHandler.RestoreValues(e.AffectedItem);
			else if (_recycleBinHandler.IsInTrash(e.Destination))
				_recycleBinHandler.ExpireTrashedItem(e.AffectedItem);
		}

		private void OnItemDeleting(object sender, CancelItemEventArgs e)
		{
			if (_recycleBinHandler.CanThrow(e.AffectedItem))
				e.FinalAction = _recycleBinHandler.Throw;
		}

		private bool LeavingTrash(DestinationEventArgs e)
		{
			return e.AffectedItem["DeletedDate"] != null && !_recycleBinHandler.IsInTrash(e.Destination);
		}
	}
}