using System;
using System.Net;

namespace Zeus.Net
{
	/// <summary>
	/// Exception thrown when a response other than 200 is returned.
	/// </summary>
	public sealed class InvalidResponseException : ZeusException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="InvalidResponseException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		public InvalidResponseException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="InvalidResponseException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="innerException">The inner exception.</param>
		public InvalidResponseException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="InvalidResponseException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="status"></param>
		public InvalidResponseException(string message, HttpStatusCode status)
			: base(message)
		{
			HttpStatus = status;
		}

		/// <summary>
		/// Gets the HTTP status returned by the service.
		/// </summary>
		/// <value>The HTTP status.</value>
		public HttpStatusCode HttpStatus { get; private set; }
	}
}