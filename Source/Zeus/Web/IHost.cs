using System.Collections.Generic;
using Zeus.BaseLibrary.Web;

namespace Zeus.Web
{
	/// <summary>
	/// Classes implementing this interface knows about available 
	/// <see cref="Site">Sites</see> and which one is the current
	/// based on the context.
	/// </summary>
	public interface IHost
	{
		Site CurrentSite { get; }
		IList<Site> Sites { get; }
		string GetLanguageFromHostName();
		Site GetSite(Url host);
	}
}