using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeus.Examples.Items
{
	[Definition("Start Page")]
	public class StartPage : ContentItem
	{
		public override string TemplateUrl
		{
			get { return "~/Examples/UI/Views/StartPage.aspx"; }
		}
	}
}
