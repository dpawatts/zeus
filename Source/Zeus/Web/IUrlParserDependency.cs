using System;

namespace Zeus.Web
{
	/// <summary>
	/// This interface is used to inject rewriter dependency on content item 
	/// objects upon creation.
	/// </summary>
	public interface IUrlParserDependency
	{
		/// <summary>Sets the objects urlParser dependency.</summary>
		/// <param name="rewriter">The url parser to inject.</param>
		void SetUrlParser(IUrlParser parser);
	}
}
