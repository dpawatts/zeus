using System;
using Ext.Net;
using Zeus.Design.Editors;
using Zeus.Integrity;

namespace Zeus.AddIns.Blogs.ContentTypes
{
	[ContentType("Blog Year Page")]
	[RestrictParents(typeof(Blog))]
	public class BlogYear : BaseBlogPage
	{
		public override Blog CurrentBlog
		{
			get { return (Blog)Parent; }
		}

		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Icon.Calendar); }
		}

		public override string Name
		{
			get { return Year.ToString(); }
			set { Year = Convert.ToInt32(value); }
		}

		public override string Title
		{
			get { return Year.ToString(); }
			set { Year = Convert.ToInt32(value); }
		}

		[TextBoxEditor("Year", 100)]
		public virtual int Year
		{
			get { return GetDetail("Year", DateTime.Now.Year); }
			set { SetDetail("Year", value); }
		}

		public override void AddTo(ContentItem newParent)
		{
			Utility.Insert(this, newParent, "Year DESC");
		}
	}
}