using System;
using System.ComponentModel;

namespace Bermedia.Gibbons.Web.Items
{
	public enum PaymentStatus
	{
		None,
		Authorized,
		Received,
		Cancelled,

		[Description("Payment Pending Via Fax")]
		Pending
	}
}
