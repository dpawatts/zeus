using System;

namespace Zeus.AddIns.ECommerce
{
	public class ZeusECommerceException : ZeusException
	{
		public ZeusECommerceException(string message)
			: base(message)
		{

		}

		public ZeusECommerceException(string messageFormat, params object[] args)
			: base(messageFormat, args)
		{

		}

		public ZeusECommerceException(string message, Exception innerException)
			: base(message, innerException)
		{

		}
	}
}