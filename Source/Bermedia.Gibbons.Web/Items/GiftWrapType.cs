using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType("Gift Wrap Type")]
	[RestrictParents(typeof(GiftWrapTypeContainer))]
	public class GiftWrapType : BaseContentItem
	{
		[LiteralDisplayer(Title = "Name")]
		[TextBoxEditor("Name", 10, Required = true)]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}

		protected override string IconName
		{
			get { return "package"; }
		}
	}
}
