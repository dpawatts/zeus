using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeus.Integrity;

namespace Zeus.Examples.Items
{
	[ContentType("Start Page")]
	[RestrictParents(AllowedTypes.None)]
	public class StartPage : ContentItem
	{
		public override string TemplateUrl
		{
			get { return "~/Examples/UI/Views/StartPage.aspx"; }
		}
	}
}
