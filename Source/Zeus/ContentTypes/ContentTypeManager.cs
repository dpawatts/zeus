using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Zeus.ContentTypes.Properties;
using System.IO;
using System.Web;
using System.Diagnostics;
using System.Security.Principal;

namespace Zeus.ContentTypes
{
	public class ContentTypeManager : IContentTypeManager
	{
		#region Fields

		private IDictionary<Type, ContentType> _definitions;

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

		public ContentTypeManager(IContentTypeBuilder contentTypeBuilder)
		{
			_definitions = contentTypeBuilder.GetDefinitions();
		}

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
			item.Parent = parentItem;
		}

		public ICollection<ContentType> GetContentTypes()
		{
			return _definitions.Values;
		}

		public ContentType GetContentType(Type type)
		{
			if (_definitions.ContainsKey(type))
				return _definitions[type];
			else
				return null;
		}

		public IList<ContentType> GetAllowedChildren(ContentType contentType, IPrincipal user)
		{
			List<ContentType> allowedChildren = new List<ContentType>();

			foreach (ContentType childItem in contentType.AllowedChildren)
			{
				if (!childItem.IsDefined)
					continue;
				if (!childItem.IsAuthorized(user))
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
