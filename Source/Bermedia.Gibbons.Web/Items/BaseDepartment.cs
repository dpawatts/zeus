using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;
using System.Web;

namespace Bermedia.Gibbons.Web.Items
{
	public abstract class BaseDepartment : StructuralPage
	{
		[HtmlTextBoxEditor("Text", 100, ContainerName = Tabs.General)]
		public string Text
		{
			get { return GetDetail<string>("Text", string.Empty); }
			set { SetDetail<string>("Text", value); }
		}

		protected override string IconName
		{
			get { return "tag_purple"; }
		}

		public string GiftsUnder10Url
		{
			get { return Zeus.Web.Url.Parse(this.Url).AppendSegment("gifts-under-10").ToString(); }
		}

		public string GiftsUnder20Url
		{
			get { return Zeus.Web.Url.Parse(this.Url).AppendSegment("gifts-under-20").ToString(); }
		}

		public string GiftsUnder50Url
		{
			get { return Zeus.Web.Url.Parse(this.Url).AppendSegment("gifts-under-50").ToString(); }
		}

		public string Action
		{
			get;
			set;
		}

		public int PriceLimit
		{
			get;
			set;
		}

		public string SearchText
		{
			get;
			set;
		}

		public override ContentItem GetChild(string childName)
		{
			if (childName.Equals("gifts-under-10", StringComparison.CurrentCultureIgnoreCase))
			{
				this.Action = "gifts";
				this.PriceLimit = 10;
				return this;
			}
			else if (childName.Equals("gifts-under-20", StringComparison.CurrentCultureIgnoreCase))
			{
				this.Action = "gifts";
				this.PriceLimit = 20;
				return this;
			}
			else if (childName.Equals("gifts-under-50", StringComparison.CurrentCultureIgnoreCase))
			{
				this.Action = "gifts";
				this.PriceLimit = 50;
				return this;
			}
			else if (childName.Equals("search", StringComparison.CurrentCultureIgnoreCase))
			{
				this.Action = "search";
				return this;
			}
			return base.GetChild(childName);
		}

		public override string TemplateUrl
		{
			get
			{
				switch (Action)
				{
					case "gifts":
						return "~/UI/Views/DepartmentGifts.aspx?PriceLimit=" + this.PriceLimit;
					case "search" :
						return "~/UI/Views/SearchDepartment.aspx";
					default:
						return base.TemplateUrl;
				}
			}
		}
	}
}
