using Coolite.Ext.Web;
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
		private const string CATEGORY_CONTAINER_NAME = "categories";
		private const string FILES_NAME = "files";

		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Icon.UserComment); }
		}

		public CategoryContainer Categories
		{
			get { return GetChild(CATEGORY_CONTAINER_NAME) as CategoryContainer; }
		}

		public Folder Files
		{
			get { return GetChild(FILES_NAME) as Folder; }
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

		void ISelfPopulator.Populate()
		{
			CategoryContainer categories = new CategoryContainer
			{
				Name = CATEGORY_CONTAINER_NAME,
				Title = "Categories"
			};
			categories.AddTo(this);

			Folder files = new Folder
			{
				Name = FILES_NAME,
				Title = "Files"
			};
			files.AddTo(this);
		}
	}
}