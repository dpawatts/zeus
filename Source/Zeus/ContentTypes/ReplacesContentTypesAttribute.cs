using System;
using System.Collections.Generic;

namespace Zeus.ContentTypes
{
	/// <summary>
	/// Disables the specified content type(s). Can be used to create a better 
	/// implementation of a content type in an existing solution. Note that this
	/// attribute doesn't modify any existing data. It only removes types from 
	/// the items that can be created.
	/// </summary>
	public class ReplacesContentTypesAttribute : AbstractContentTypeRefiner, IDefinitionRefiner
	{
		private readonly Type[] _replacedContentTypes;

		public ReplacesContentTypesAttribute(params Type[] replacedContentTypes)
		{
			_replacedContentTypes = replacedContentTypes;
		}

		public ReplacesContentTypesAttribute(Type replacedContentTypes)
		{
			_replacedContentTypes = new[] { replacedContentTypes };
		}

		public override void Refine(ContentType currentContentType, IList<ContentType> allContentTypes)
		{
			foreach (ContentType contentType in allContentTypes)
				foreach (Type t in _replacedContentTypes)
					if (contentType.ItemType == t)
						contentType.Enabled = false;
		}
	}
}