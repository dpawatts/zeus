using System;
using Zeus.Integrity;

namespace Zeus.Templates.ContentTypes.News
{
	[ContentType("News Section")]
	[RestrictParents(typeof(StartPage))]
	public class NewsContainer : BaseNewsPage
	{
		protected override string IconName
		{
			get { return "newspaper"; }
		}

		public override NewsContainer CurrentNewsContainer
		{
			get { return this; }
		}

		public NewsMonth CurrentMonth
		{
			get
			{
				NewsYear year = GetChild(DateTime.Today.ToString("yyyy")) as NewsYear;
				if (year == null)
					return null;

				return year.GetChild(DateTime.Today.ToString("MM")) as NewsMonth;
			}
		}
	}
}