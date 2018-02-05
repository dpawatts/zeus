using System.Collections.Generic;

namespace Zeus.Admin.RecycleBin
{
	/// <summary>
	/// Container of scrapped items.
	/// </summary>
	public interface IRecycleBin
	{
		/// <summary>Whether the trash functionality is enabled.</summary>
		bool Enabled { get; }

		/// <summary>Currently thrown items.</summary>
		IList<ContentItem> Children { get; }
	}
}