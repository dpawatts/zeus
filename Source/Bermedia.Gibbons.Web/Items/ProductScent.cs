using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;
using System.Web.UI.WebControls;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType(Description = "Product Scent")]
	public class ProductScent : ProductColour
	{
		[TextBoxEditor("Description", 20, TextMode = TextBoxMode.MultiLine)]
		public virtual string Description
		{
			get { return GetDetail<string>("Description", string.Empty); }
			set { SetDetail<string>("Description", value); }
		}

		protected override string IconName
		{
			get { return "ipod"; }
		}
	}
}
