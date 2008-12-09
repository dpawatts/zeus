using System;
using Zeus;
using Zeus.Integrity;

namespace Bermedia.Gibbons.Items
{
	[ContentType(Description = "e.g. Calvin Klein Striped Socks, Must de Cartier Eau de Toilette")]
	[RestrictParents(typeof(Category))]
	public class Product : StructuralPage
	{
		protected override string IconName
		{
			get { return "tag_orange"; }
		}
	}
}
