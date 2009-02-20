using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;
using Zeus.FileSystem;
using System.Web.UI.WebControls;
using Zeus.Web.UI;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType("Product", Description = "e.g. Calvin Klein Striped Socks, Must de Cartier Eau de Toilette")]
	[RestrictParents(typeof(NonFragranceBeautyCategory))]
	public class NonFragranceBeautyProduct : StandardProduct
	{
		#region Public properties

		[LinkedItemDropDownListEditor("Brand", 205, TypeFilter = typeof(Brand), ContainerName = Tabs.General)]
		public override Brand Brand
		{
			get { return GetDetail<Brand>("Brand", null); }
			set { SetDetail<Brand>("Brand", value); }
		}

		#endregion
	}
}
