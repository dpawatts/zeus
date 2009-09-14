using System;
using System.Collections.Generic;

namespace Zeus.Templates.Services.Syndication
{
	/// <summary>
	/// Items implementing this interface provide feed meta data and syndicated 
	/// items.
	/// </summary>
	public interface IFeed
	{
		int NumberOfItems { get; set; }
		string Tagline { get; set; }
		string Author { get; set; }
		string Title { get; set; }
		string Url { get; }
		DateTime? Published { get; }

		IEnumerable<ISyndicatable> GetItems();
	}
}