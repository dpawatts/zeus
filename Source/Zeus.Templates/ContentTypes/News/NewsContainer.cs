using System;
using Coolite.Ext.Web;
using Zeus.Integrity;
using Zeus.Web;

namespace Zeus.Templates.ContentTypes.News
{
	[ContentType("News Section")]
	[RestrictParents(typeof(WebsiteNode))]
	public class NewsContainer : BaseNewsPage
	{
		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Icon.Newspaper); }
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