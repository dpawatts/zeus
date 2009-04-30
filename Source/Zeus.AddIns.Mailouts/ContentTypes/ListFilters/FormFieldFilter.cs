using Zeus.AddIns.Mailouts.ContentTypes.FormFields;
using Zeus.AddIns.Mailouts.Services;

namespace Zeus.AddIns.Mailouts.ContentTypes.ListFilters
{
	public abstract class FormFieldFilter : ListFilter
	{
		public virtual FormField FormField
		{
			get { return GetDetail<FormField>("FormField", null); }
			set { SetDetail("FormField", value); }
		}
	}
}