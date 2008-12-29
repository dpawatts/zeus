using System;
using System.ComponentModel;

namespace Bermedia.Gibbons.Web.Items
{
	public enum NewsletterStatus
	{
		[Description("Not Started")]
		NotStarted,

		[Description("Running Report")]
		RunningReport,

		[Description("Mailout In Progress")]
		InProgress,

		Successful,
		Failed
	}
}
