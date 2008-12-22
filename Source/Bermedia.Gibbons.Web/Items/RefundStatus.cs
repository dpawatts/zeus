using System;
using System.ComponentModel;

namespace Bermedia.Gibbons.Web.Items
{
	public enum RefundStatus
	{
		[Description("Not Refunded")]
		NotRefunded,

		[Description("Partially Refunded")]
		PartiallyRefunded,

		Refunded
	}
}
