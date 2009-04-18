﻿using System;
using System.Linq;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType(Description = "[Internal Use Only]")]
	[RestrictParents(typeof(GiftCardPage))]
	public class GiftCardPersonalisePage : StructuralPage, ITopNavVisible
	{
		protected override string IconName
		{
			get { return "page"; }
		}

		protected override string TemplateName
		{
			get { return "GiftCardPersonalise"; }
		}
	}
}
