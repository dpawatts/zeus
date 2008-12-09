using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeus;
using Zeus.Integrity;

namespace Bermedia.Gibbons.Items
{
	[ContentType("Start Page")]
	[RestrictParents(AllowedTypes.None)]
	public class StartPage : Page
	{
		protected override string TemplateName
		{
			get { return "Page"; }
		}
	}
}
