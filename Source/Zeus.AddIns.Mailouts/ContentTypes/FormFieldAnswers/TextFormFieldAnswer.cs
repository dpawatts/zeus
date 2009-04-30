using Zeus.AddIns.Mailouts.ContentTypes.FormFields;
using Zeus.Design.Editors;

namespace Zeus.AddIns.Mailouts.ContentTypes.FormFieldAnswers
{
	[ContentType("Text Field Answer")]
	public class TextFormFieldAnswer : FormFieldAnswer
	{
		[LinkedItemDropDownListEditor("Form Field", 10, Required = true, TypeFilter = typeof(TextFormField))]
		public override FormField FormField
		{
			get { return base.FormField; }
			set { base.FormField = value; }
		}

		[TextBoxEditor("Value", 20)]
		public override object Value
		{
			get { return base.Value; }
			set { base.Value = value; }
		}
	}
}