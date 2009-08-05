using System;

namespace Protx.Vsp
{
	public class VspException : Exception
	{
		public VspException()
		{
		}

		public VspException(string message) : base(message)
		{
		}

		public VspException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}