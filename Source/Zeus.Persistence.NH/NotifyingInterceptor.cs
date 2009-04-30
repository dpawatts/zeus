using NHibernate;
using NHibernate.Type;

namespace Zeus.Persistence.NH
{
	/// <summary>
	/// This class is used to notify subscribers about loaded items.
	/// </summary>
	public class NotifyingInterceptor : EmptyInterceptor, INotifyingInterceptor
	{
		private readonly IItemNotifier _notifier;

		public NotifyingInterceptor(IItemNotifier notifier)
		{
			_notifier = notifier;
		}

		/// <summary>Sets rewriter and definition manager on a content item object at load time.</summary>
		/// <param name="entity">The potential content item whose definition manager and rewriter will be set.</param>
		/// <param name="id">Ignored.</param>
		/// <param name="state">Ignored.</param>
		/// <param name="propertyNames">Ignored.</param>
		/// <param name="types">Ignored.</param>
		/// <returns>True if the entity was a content item.</returns>
		public override bool OnLoad(object entity, object id, object[] state, string[] propertyNames, IType[] types)
		{
			return _notifier.Notify(entity as ContentItem);
		}
	}
}
