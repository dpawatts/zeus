using Zeus.AddIns.Mailouts.ContentTypes.FormFields;
using Zeus.AddIns.Mailouts.Services;
using Zeus.Design.Editors;
using Zeus.Integrity;

namespace Zeus.AddIns.Mailouts.ContentTypes.FormFieldAnswers
{
	[RestrictParents(typeof(IMailoutRecipient))]
	public abstract class FormFieldAnswer : ContentItem
	{
		public virtual FormField FormField
		{
			get { return GetDetail<FormField>("FormField", null); }
			set { SetDetail("FormField", value); }
		}

		public virtual object Value
		{
			get { return GetDetail<object>("Value", null); }
			set { SetDetail("Value", value); }
		}
	}
}