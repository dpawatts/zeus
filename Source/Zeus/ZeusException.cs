using System;

namespace Zeus
{
	/// <summary>The base class for Zeus application exceptions.</summary>
	public class ZeusException : ApplicationException
	{
		/// <summary>Creates a new instance of the ZeusException, the base class for known Zeus exceptions.</summary>
		/// <param name="message">The exception message</param>
		public ZeusException(string message)
			: base(message)
		{
		}

		/// <summary>Creates a new instance of the ZeusException, the base class for known Zeus exceptions.</summary>
		/// <param name="messageFormat">The exception message format.</param>
		/// <param name="args">The exception message arguments.</param>
		public ZeusException(string messageFormat, params object[] args)
			: base(string.Format(messageFormat, args))
		{
		}

		/// <summary>Creates a new instance of the ZeusException that encapsulates an underlying exception.</summary>
		/// <param name="message">The exception message.</param>
		/// <param name="innerException">The underlying exception.</param>
		public ZeusException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
