using System;

namespace Zeus.Templates.Services.Syndication
{
	/// <summary>
	/// Items implementing this interface provide the information needed by the
	/// <see cref="RssWriter"/> to write a feed.
	/// </summary>
	public interface ISyndicatable
	{
		string Title { get; }
		string Url { get; }
		string Summary { get; }
		DateTime Published { get; }
	}
}