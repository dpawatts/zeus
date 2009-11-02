using Zeus.Integrity;
using Zeus.Design.Editors;
using Coolite.Ext.Web;
using System;

namespace Zeus.Templates.ContentTypes.ReferenceData
{
	[ContentType("Country")]
	[RestrictParents(typeof(CountryList))]
	public class Country : BaseContentItem
	{
		public Country()
		{

		}

		public Country(string alpha2, string ignore, string title, string alpha3, string numeric)
		{
			this.Alpha2 = alpha2;
			this.Title = title;
			this.Alpha3 = alpha3;
			this.Numeric = numeric;

			string tempIconName = "Flag" + alpha2.Substring(0, 1) + alpha2.Substring(1).ToLower();
			Icon icon;
			if (Enum.TryParse<Icon>(tempIconName, out icon))
				Icon = icon;
		}

		[TextBoxEditor("Name", 10, Required = true)]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}

		public string Numeric
		{
			get { return GetDetail<string>("Numeric", null); }
			set { SetDetail<string>("Numeric", value); }
		}

		public string Alpha2
		{
			get { return GetDetail<string>("Alpha2", null); }
			set { SetDetail<string>("Alpha2", value); }
		}

		public string Alpha3
		{
			get { return GetDetail<string>("Alpha3", null); }
			set { SetDetail<string>("Alpha3", value); }
		}

		public Icon Icon
		{
			get { return GetDetail<Icon>("Icon", Icon.Map); }
			set { SetDetail<Icon>("Icon", value); }
		}

		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Icon); }
		}
	}
}
