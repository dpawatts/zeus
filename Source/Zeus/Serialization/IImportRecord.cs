using System;
using System.Collections.Generic;

namespace Zeus.Serialization
{
	public interface IImportRecord
	{
		IList<ContentItem> ReadItems { get; }
		ContentItem RootItem { get; }
		IList<Exception> Errors { get; }
	}
}