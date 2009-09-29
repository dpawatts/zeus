using Zeus.ContentProperties;
using Zeus.ContentTypes;
using Zeus.Design.Editors;
using Zeus.FileSystem;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.Blogs.ContentTypes
{
	[ContentType]
	public class Blog : BasePage, ISelfPopulator, IFileSystemContainer
	{
		private const string CATEGORY_CONTAINER_NAME = "categories";
		private const string FILES_NAME = "files";

		public override string IconUrl
		{
			get { return GetIconUrl(typeof(Blog), "Zeus.AddIns.Blogs.Icons.user_comment.png"); }
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