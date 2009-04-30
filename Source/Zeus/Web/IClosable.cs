using System;

namespace Zeus.Web
{
	/// <summary>
	/// Represents a class that can end and be disposed. Used to mark classes in 
	/// the request context that may be disposed when the request ends.
	/// </summary>
	public interface IClosable : IDisposable
	{
	}
}