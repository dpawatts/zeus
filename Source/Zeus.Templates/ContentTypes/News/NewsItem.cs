using System;
using Coolite.Ext.Web;
using Zeus.ContentProperties;
using Zeus.Design.Editors;
using Zeus.FileSystem;
using Zeus.Integrity;
using Zeus.Templates.Services.Syndication;
using Image = Zeus.FileSystem.Images.Image;

namespace Zeus.Templates.ContentTypes.News
{
	[ContentType("News Item")]
	[RestrictParents(typeof(NewsContainer), typeof(NewsMonth))]
	[AllowedChildren(typeof(Image))]
	public class NewsItem : BaseNewsPage, IFileSystemContainer, ISyndicatable
	{
		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Icon.NewspaperLink); }
		}

		public override NewsContainer CurrentNewsContainer
		{
			get { return (GetParent() is NewsMonth) ? ((NewsMonth)GetParent()).CurrentNewsContainer : (NewsContainer)GetParent(); }
		}

		[ContentProperty("Date", 30)]
		[DateEditor(Required = true, ContainerName = "Content")]
		public virtual DateTime Date
		{
			get { return GetDetail("Date", DateTime.Today); }
			set { SetDetail("Date", value); }
		}

		public string FormattedDate
		{
			get { return Date.ToLongDateString(); }
		}

		[XhtmlStringContentProperty("Introduction", 35)]
		public virtual string Introduction
		{
			get { return GetDetail("Introduction", string.Empty); }
			set { SetDetail("Introduction", value); }
		}

		[ContentProperty("Content", 40, Shared = false)]
		[HtmlTextBoxEditor(ContainerName = "Content")]
		public virtual string Content
		{
			get { return GetDetail("Content", string.Empty); }
			set { SetDetail("Content", value); }
		}

		[MultiImageUploadEditor("Images", 50, ContainerName = "Content")]
		public virtual PropertyCollection Images
		{
			get { return GetDetailCollection("Images", true); }
		}

		public override void AddTo(ContentItem newParent)
		{
			Utility.Insert(this, newParent, "Date DESC");
		}

		DateTime ISyndicatable.Published
		{
			get { return (Published != null) ? Published.Value : DateTime.MinValue; }
		}

		string ISyndicatable.Summary
		{
			get { return Introduction; }
		}
	}
}