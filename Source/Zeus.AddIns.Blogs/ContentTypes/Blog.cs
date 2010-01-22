using Ext.Net;
using Zeus.ContentTypes;
using Zeus.FileSystem;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;
using Zeus.Web;

namespace Zeus.AddIns.Blogs.ContentTypes
{
	[ContentType]
	[RestrictParents(typeof(WebsiteNode), typeof(Page))]
	public class Blog : BasePage, ISelfPopulator, IFileSystemContainer, ITagGroupContainer
	{
		private const string CategoryContainerName = "categories";
		private const string FilesName = "files";

		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Icon.UserComment); }
		}

		public CategoryContainer Categories
		{
			get { return GetChild(CategoryContainerName) as CategoryContainer; }
		}

		public Folder Files
		{
			get { return GetChild(FilesName) as Folder; }
		}

		[ContentProperty("Pingbacks Enabled?", 200)]
		public virtual bool PingbacksEnabled
		{
			get { return GetDetail("PingbacksEnabled", true); }
			set { SetDetail("PingbacksEnabled", value); }
		}

		[ContentProperty("Comment Moderation Enabled?", 210, Description = "If this box is checked, an administrator must always approve comments before they appear on the site.")]
		public virtual bool CommentModerationEnabled
		{
			get { return GetDetail("CommentModerationEnabled", true); }
			set { SetDetail("CommentModerationEnabled", value); }
		}

		[ContentProperty("Page Size", 220, Description = "Posts per page for the post listing pages.")]
		public virtual int PageSize
		{
			get { return GetDetail("PageSize", 10); }
			set { SetDetail("PageSize", value); }
		}

		void ISelfPopulator.Populate()
		{
			CategoryContainer categories = new CategoryContainer
			{
				Name = CategoryContainerName,
				Title = "Categories"
			};
			categories.AddTo(this);

			Folder files = new Folder
			{
				Name = FilesName,
				Title = "Files"
			};
			files.AddTo(this);
		}
	}
}