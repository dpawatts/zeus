using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace Zeus.ContentTypes
{
	public interface IContentTypeManager
	{
		ContentType this[Type type]
		{
			get;
		}

		ContentType this[string discriminator]
		{
			get;
		}

		ContentItem CreateInstance(Type itemType, ContentItem parentItem);
		T CreateInstance<T>(ContentItem parentItem)
			where T : ContentItem;

		ICollection<ContentType> GetContentTypes();
		ContentType GetContentType(Type type);
		ContentType GetContentType(string discriminator);
		IList<ContentType> GetAllowedChildren(ContentType contentType, IPrincipal user);
	}
}
