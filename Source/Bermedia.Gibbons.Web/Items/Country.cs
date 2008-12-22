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
		[TextBoxEditor("Name", 10, Required = true)]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}

		protected override string IconName
		{
			get { return "ipod"; }
		}
	}
}
