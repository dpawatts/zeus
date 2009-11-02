using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coolite.Ext.Web;
using Zeus.Integrity;

namespace Zeus.Templates.ContentTypes
{
	[ContentType("System Node")]
	[RestrictParents(typeof(RootItem))]
	public class SystemNode : BaseContentItem
	{
		public override string Title
		{
			get { return "System"; }
			set { base.Title = value; }
		}

		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Icon.Application); }
		}
	}
}
