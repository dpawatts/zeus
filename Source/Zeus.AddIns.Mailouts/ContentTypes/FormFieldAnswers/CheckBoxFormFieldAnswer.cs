using Zeus.AddIns.Mailouts.ContentTypes.FormFields;
using Zeus.Design.Editors;

namespace Zeus.AddIns.Mailouts.ContentTypes.FormFieldAnswers
{
	[ContentType("Check Box Field Answer")]
	public class CheckBoxFormFieldAnswer : FormFieldAnswer
	{
		[LinkedItemDropDownListEditor("Form Field", 10, Required = true, TypeFilter = typeof(CheckBoxFormField))]
		public override FormField FormField
		{
			get { return base.FormField; }
			set { base.FormField = value; }
		}

		[CheckBoxEditor("Value", "", 20)]
		public override object Value
		{
			get { return base.Value; }
			set { base.Value = value; }
		}
	}
}