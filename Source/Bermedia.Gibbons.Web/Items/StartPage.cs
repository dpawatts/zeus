using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeus;
using Zeus.Integrity;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType("Start Page")]
	[RestrictParents(typeof(RootItem))]
	public class StartPage : Page
	{
		protected override string IconName
		{
			get { return "house"; }
		}

		protected override string TemplateName
		{
			get { return "Page"; }
		}
	}
}
