using System;
using Isis.Web;

namespace Zeus.AddIns.Blogs.Services.Tracking
{
	/// <summary>
	/// Implements http://hixie.ch/specs/pingback/pingback-1.0.
	/// </summary>
	public interface IPingbackService
	{
		Url GetPingbackUrl(Url destination);
		string SendPing(Url pingbackUrl, Url source, Url destination);
	}
}