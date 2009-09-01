using System;
using Zeus.Design.Editors;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes.News;

namespace Zeus.AddIns.Blogs.ContentTypes
{
	[ContentType("Blog Month Page")]
	[RestrictParents(typeof(BlogYear))]
	public class BlogMonth : BaseBlogPage
	{
		public override Blog CurrentBlog
		{
			get { return ((BlogYear)Parent).CurrentBlog; }
		}

		public override string IconUrl
		{
			get { return GetIconUrl(typeof(BlogMonth), "Zeus.AddIns.Blogs.Icons.calendar_view_month.png"); }
		}

		[TextBoxEditor("Name", 20)]
		public override string Name
		{
			get { return base.Name; }
			set
			{
				base.Name = value;
				Month = Convert.ToInt32(value);
			}
		}

		[TextBoxEditor("Month", 30)]
		public virtual int Month
		{
			get { return GetDetail("Month", DateTime.Now.Month); }
			set { SetDetail("Month", value); }
		}

		/// <summary>
		/// Date represented by this month and its parent year. Day is set to 1.
		/// </summary>
		public DateTime Date
		{
			get { return new DateTime(((BlogYear) Parent).Year, Month, 1); }
		}

		public override void AddTo(ContentItem newParent)
		{
			Utility.Insert(this, newParent, "Month DESC");
		}
	}
}