using System;
using System.Linq;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;
using System.Web;

namespace Bermedia.Gibbons.Web.Items
{
	public abstract class BaseDepartment : StructuralPage, ITopNavVisible
	{
		[HtmlTextBoxEditor("Text", 100, ContainerName = Tabs.General)]
		public string Text
		{
			get { return GetDetail<string>("Text", string.Empty); }
			set { SetDetail<string>("Text", value); }
		}

		[CheckBoxEditor("Show Page If Child Departments Exist", "", 120, ContainerName = Tabs.General)]
		public bool ShowPageIfChildDepartmentsExist
		{
			get { return GetDetail("ShowPageIfChildDepartmentsExist", false); }
			set { SetDetail("ShowPageIfChildDepartmentsExist", value); }
		}

		protected override string IconName
		{
			get { return "tag_purple"; }
		}

		public string GiftsUnder25Url
		{
			get { return Zeus.Web.Url.Parse(this.Url).AppendSegment("gifts-under-25").ToString(); }
		}

		public string GiftsUnder50Url
		{
			get { return Zeus.Web.Url.Parse(this.Url).AppendSegment("gifts-under-50").ToString(); }
		}

		public string GiftsUnder100Url
		{
			get { return Zeus.Web.Url.Parse(this.Url).AppendSegment("gifts-under-100").ToString(); }
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
			if (childName.Equals("gifts-under-25", StringComparison.CurrentCultureIgnoreCase))
			{
				this.Action = "gifts";
				this.PriceLimit = 25;
				return this;
			}
			else if (childName.Equals("gifts-under-50", StringComparison.CurrentCultureIgnoreCase))
			{
				this.Action = "gifts";
				this.PriceLimit = 50;
				return this;
			}
			else if (childName.Equals("gifts-under-100", StringComparison.CurrentCultureIgnoreCase))
			{
				this.Action = "gifts";
				this.PriceLimit = 100;
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

		protected override string TemplateName
		{
			get { return "Department"; }
		}
	}
}
