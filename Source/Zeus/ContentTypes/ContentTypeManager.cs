using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Zeus.Persistence;
using Zeus.Security;

namespace Zeus.ContentTypes
{
	public class ContentTypeManager : IContentTypeManager
	{
		#region Fields

		private readonly IDictionary<Type, ContentType> _definitions;
		private readonly IItemNotifier _notifier;

		#endregion

		#region Properties

		public ContentType this[Type type]
		{
			get { return GetContentType(type); }
		}

		public ContentType this[string discriminator]
		{
			get { return GetContentType(discriminator); }
		}

		#endregion

		#region Constructor

		public ContentTypeManager(IContentTypeBuilder contentTypeBuilder, IItemNotifier notifier)
		{
			_definitions = contentTypeBuilder.GetDefinitions();

			// Verify that content types have unique names.
			List<string> discriminators = new List<string>();
			foreach (ContentType contentType in _definitions.Values)
			{
				if (discriminators.Contains(contentType.Discriminator))
					throw new ZeusException("Duplicate content type discriminator. The discriminator '{0}' is already in use.", contentType.Discriminator);
				discriminators.Add(contentType.Discriminator);
			}

			_notifier = notifier;
		}

		#endregion

		#region Events

		/// <summary>Notifies subscriber that an item was created through a <see cref="CreateInstance"/> method.</summary>
		public event EventHandler<ItemEventArgs> ItemCreated;

		#endregion

		#region Methods

		public T CreateInstance<T>(ContentItem parentItem)
			where T : ContentItem
		{
			T item = Activator.CreateInstance<T>();
			OnItemCreating(item, parentItem);
			return item;
		}

		/// <summary>Creates an instance of a certain type of item. It's good practice to create new items through this method so the item's dependencies can be injected by the engine.</summary>
		/// <returns>A new instance of an item.</returns>
		public ContentItem CreateInstance(Type itemType, ContentItem parentItem)
		{
			ContentItem item = Activator.CreateInstance(itemType) as ContentItem;
			OnItemCreating(item, parentItem);
			return item;
		}

		protected virtual void OnItemCreating(ContentItem item, ContentItem parentItem)
		{
			if (parentItem != null)
			{
				ContentType parentDefinition = GetContentType(parentItem.GetType());
				ContentType itemDefinition = GetContentType(item.GetType());

				if (!parentDefinition.IsChildAllowed(itemDefinition))
					throw new NotAllowedParentException(itemDefinition, parentItem.GetType());

				item.Parent = parentItem;
				foreach (AuthorizationRule rule in parentItem.AuthorizationRules)
					item.AuthorizationRules.Add(new AuthorizationRule(item, rule.Operation, rule.Role, rule.User, rule.Allowed));
			}

			if (item is ISelfPopulator)
				((ISelfPopulator) item).Populate();

			_notifier.Notify(item);

			if (ItemCreated != null)
				ItemCreated.Invoke(this, new ItemEventArgs(item));
		}

		public ICollection<ContentType> GetContentTypes()
		{
			return _definitions.Values;
		}

		public ContentType GetContentType(Type type)
		{
			if (_definitions.ContainsKey(type))
				return _definitions[type];
			return null;
		}

		public IList<ContentType> GetAllowedChildren(ContentType contentType, string zone, IPrincipal user)
		{
			if (!contentType.HasZone(zone))
				throw new ZeusException("The content type '{0}' does not allow a zone named '{1}'.", contentType.Title, zone);

			List<ContentType> allowedChildren = new List<ContentType>();
			foreach (ContentType childItem in contentType.AllowedChildren)
			{
				if (!childItem.IsDefined)
					continue;
				if (!childItem.Enabled)
					continue;
				if (!childItem.IsAuthorized(user))
					continue;
				if (!contentType.IsAllowedInZone(zone))
					continue;
				allowedChildren.Add(childItem);
			}
			allowedChildren.Sort();
			return allowedChildren;
		}

		public ContentType GetContentType(string discriminator)
		{
			return _definitions.Values.SingleOrDefault(ct => ct.Discriminator == discriminator);
		}

		#endregion
	}
}