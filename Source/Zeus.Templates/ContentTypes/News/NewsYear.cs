using System;
using Zeus.Design.Editors;
using Zeus.Integrity;
using Zeus.Web.UI.WebControls;

namespace Zeus.Templates.ContentTypes.News
{
	[ContentType("News Year Page")]
	[RestrictParents(typeof(NewsContainer))]
	public class NewsYear : BaseNewsPage, IBreadcrumbAppearance
	{
		public override NewsContainer CurrentNewsContainer
		{
			get { return (NewsContainer) Parent; }
		}

		protected override string IconName
		{
			get { return "newspaper"; }
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

		#region IBreadcrumbAppearance Members

		bool IBreadcrumbAppearance.VisibleInBreadcrumb
		{
			get { return false; }
		}

		#endregion
	}
}