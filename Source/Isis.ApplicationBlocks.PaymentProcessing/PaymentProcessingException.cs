using System;
using System.Collections.Specialized;

namespace Isis.ApplicationBlocks.PaymentProcessing
{
	public class PaymentProcessingException : Exception
	{
		private int mReasonCode;
		private int mResponseCode;
		private string mReasonCodeDesription;
		private string mOrderID;

		public override string Message
		{
			get
			{
				return mReasonCodeDesription;
			}
		}

		public string OrderID
		{
			get
			{
				return mOrderID;
			}
		}

		public int ErrorCode
		{
			get
			{
				return mReasonCode;
			}
		}

		public int Result
		{
			get
			{
				return mResponseCode;
			}
		}

		public PaymentProcessingException(NameValueCollection parsedResponse)
		{
			mResponseCode = Convert.ToInt32(parsedResponse["ResponseCode"]);
			mReasonCode = Convert.ToInt32(parsedResponse["ReasonCode"]);
			mReasonCodeDesription = parsedResponse["ReasonCodeDesc"];
			mOrderID = parsedResponse["OrderID"];
		}
	}
}