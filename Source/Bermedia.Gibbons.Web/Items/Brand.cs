using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;

namespace Bermedia.Gibbons.Items
{
	[ContentType(Description = "Product brands")]
	[RestrictParents(typeof(BrandContainer))]
	public class Brand : BaseContentItem
	{
		[TextBoxEditor("Name", 10, ContainerName = Tabs.General, Required = true)]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}

		protected override string IconName
		{
			get { return "ipod"; }
		}
	}
}
