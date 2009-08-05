using System;

namespace Isis.Web
{
	/// <summary>
	/// Thrown when attempting to decrypt or deserialize an invalid encrypted queryString.
	/// </summary>
	public class InvalidQueryStringException : System.Exception
	{
		public InvalidQueryStringException() : base() { }
	}
}
