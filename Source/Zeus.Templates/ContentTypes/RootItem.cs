using System;
using Zeus.ContentTypes;
using Zeus.FileSystem.Images;
using Zeus.Globalization.ContentTypes;
using Zeus.Integrity;

namespace Zeus.Templates.ContentTypes
{
	[ContentType("Root Item")]
	[RestrictParents(AllowedTypes.None)]
	public class RootItem : BaseContentItem, IRootItem
	{
		protected override string IconName
		{
			get { return "page_gear"; }
		}
	}
}