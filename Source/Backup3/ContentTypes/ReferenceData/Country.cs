using Ext.Net;
using Zeus.BaseLibrary;
using Zeus.Integrity;
using Zeus.Design.Editors;

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
			if (EnumHelper.TryParse(tempIconName, out icon))
				FlagIcon = icon;
		}

		[TextBoxEditor("Name", 10, Required = true)]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}

		[TextBoxEditor("CountryCode", 20, Required = false)]
		public string CountryCode
		{
			get { return GetDetail<string>("CountryCode", null); }
			set { SetDetail("CountryCode", value); }
		}

		public string Numeric
		{
			get { return GetDetail<string>("Numeric", null); }
			set { SetDetail("Numeric", value); }
		}

		public string Alpha2
		{
			get { return GetDetail<string>("Alpha2", null); }
			set { SetDetail("Alpha2", value); }
		}

		public string Alpha3
		{
			get { return GetDetail<string>("Alpha3", null); }
			set { SetDetail("Alpha3", value); }
		}

		public Icon FlagIcon
		{
			get { return GetDetail("FlagIcon", Icon.Map); }
			set { SetDetail("FlagIcon", value); }
		}

		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(FlagIcon); }
		}
	}
}
