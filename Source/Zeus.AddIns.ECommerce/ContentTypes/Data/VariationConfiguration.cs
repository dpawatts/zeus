using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;
using Zeus.Web;

namespace Zeus.AddIns.ECommerce.ContentTypes.Data
{
	[ContentType]
	[RestrictParents(typeof(Product))]
	public class VariationConfiguration : BaseContentItem, ILink
	{
		public override string IconUrl
		{
			get { return GetIconUrl(typeof(VariationConfiguration), "Zeus.AddIns.ECommerce.Icons.arrow_merge.png"); }
		}

		string ILink.Contents
		{
			get { return ((ILink) Permutation).Contents; }
		}

		public VariationPermutation Permutation
		{
			get { return GetChild("permutation") as VariationPermutation; }
			set
			{
				if (value != null)
				{
					value.Name = "permutation";
					value.AddTo(this);
				}
			}
		}

		public bool Available
		{
			get { return GetDetail("Available", false); }
			set { SetDetail("Available", value); }
		}
	}
}