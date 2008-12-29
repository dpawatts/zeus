using System;
using System.ComponentModel;

namespace Bermedia.Gibbons.Web.Items
{
	public enum NewsletterLogEntryStatus
	{
		[Description("Not Attempted")]
		NotAttempted,

		Queued,
		Failed
	}
}
