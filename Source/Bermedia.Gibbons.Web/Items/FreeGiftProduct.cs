using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeus;
using Zeus.Integrity;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType("Free Gift Product", Description = "e.g. Complimentary Perfume")]
	[RestrictParents(typeof(BaseCategory))]
	public class FreeGiftProduct : BaseProduct
	{
		protected override string IconName
		{
			get { return "tag_orange"; }
		}
	}
}
