using System;
using Zeus.Design.Editors;
using Zeus.Integrity;

namespace Zeus.Templates.ContentTypes.News
{
	[ContentType("News Month Page")]
	[RestrictParents(typeof(NewsYear))]
	public class NewsMonth : BaseNewsPage
	{
		public override NewsContainer CurrentNewsContainer
		{
			get { return ((NewsYear) Parent).CurrentNewsContainer; }
		}

		protected override string IconName
		{
			get { return "newspaper"; }
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

		public override void AddTo(ContentItem newParent)
		{
			Utility.Insert(this, newParent, "Year DESC");
		}
	}
}