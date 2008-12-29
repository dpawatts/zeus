using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType(Description = "Country")]
	[RestrictParents(typeof(CountryContainer))]
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

		protected override string IconName
		{
			get { return "ipod"; }
		}
	}
}
