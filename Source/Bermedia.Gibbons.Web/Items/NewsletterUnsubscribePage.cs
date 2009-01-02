using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeus;
using Zeus.Integrity;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType(Description = "[Internal Use Only]")]
	[RestrictParents(typeof(StartPage))]
	public class NewsletterUnsubscribePage : StructuralPage
	{
		protected override string IconName
		{
			get { return "page"; }
		}

		protected override string TemplateName
		{
			get { return "NewsletterUnsubscribe"; }
		}
	}
}
