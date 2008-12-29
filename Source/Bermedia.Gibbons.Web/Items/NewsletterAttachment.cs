using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;
using Zeus.FileSystem;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType(Title = "Newsletter Attachment")]
	[RestrictParents(typeof(Newsletter))]
	public class NewsletterAttachment : BaseContentItem
	{
		[FileUploadEditor("File", 10, "~/Upload/NewsletterAttachments")]
		public File File
		{
			get { return GetDetail<File>("File", null); }
			set { SetDetail<File>("File", value); }
		}

		protected override string IconName
		{
			get { return "newspaper"; }
		}
	}
}
