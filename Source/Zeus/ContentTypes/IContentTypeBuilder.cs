using System;
using System.Collections.Generic;

namespace Zeus.ContentTypes
{
	public interface IContentTypeBuilder
	{
		IDictionary<Type, ContentType> GetDefinitions();
	}
}
