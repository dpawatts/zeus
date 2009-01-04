using System;
using System.Linq;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType(Description = "[Internal Use Only]")]
	[RestrictParents(typeof(StartPage), typeof(GiftCardPage))]
	public class GiftCardPage : Page, ITopNavVisible
	{
		
	}
}
