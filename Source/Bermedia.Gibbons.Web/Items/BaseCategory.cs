using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;
using Zeus.AddIns.Images.Items.Details;
using SoundInTheory.DynamicImage.Filters;

namespace Bermedia.Gibbons.Web.Items
{
	public abstract class BaseCategory : StructuralPage
	{
		protected override string IconName
		{
			get { return "tag_green"; }
		}
	}
}
