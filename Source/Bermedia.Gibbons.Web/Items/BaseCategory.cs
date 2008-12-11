using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;

namespace Bermedia.Gibbons.Items
{
	public abstract class BaseCategory : StructuralPage
	{
		protected override string IconName
		{
			get { return "tag_green"; }
		}

		protected override string TemplateName
		{
			get { return "Category"; }
		}
	}
}
