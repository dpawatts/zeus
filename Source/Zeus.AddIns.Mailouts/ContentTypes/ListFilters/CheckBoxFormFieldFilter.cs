using Zeus.AddIns.Mailouts.ContentTypes.FormFields;
using Zeus.AddIns.Mailouts.Services;
using Zeus.Design.Editors;

namespace Zeus.AddIns.Mailouts.ContentTypes.ListFilters
{
	[ContentType("Checkbox Form Field Filter")]
	public class CheckBoxFormFieldFilter : FormFieldFilter
	{
		[LinkedItemDropDownListEditor("Form Field", 10, Required = true, TypeFilter = typeof(CheckBoxFormField))]
		public override FormField FormField
		{
			get { return base.FormField; }
			set { base.FormField = value; }
		}

		[EnumEditor("Operator", 20, typeof(CheckBoxFormFieldOperator))]
		public virtual int Operator
		{
			get { return GetDetail("Operator", (int) CheckBoxFormFieldOperator.Is); }
			set { SetDetail("Operator", value); }
		}

		[EnumEditor("Value", 30, typeof(CheckBoxFormFieldValue))]
		public virtual int Value
		{
			get { return GetDetail("Value", (int) CheckBoxFormFieldValue.True); }
			set { SetDetail("Value", value); }
		}

		public override bool Matches(IMailoutRecipient recipient)
		{
			bool answer = (bool) recipient.Fields[FormField.Name].Value;
			return (Value == (int) CheckBoxFormFieldValue.True) ? answer : !answer;
		}

		public enum CheckBoxFormFieldOperator
		{
			Is
		}

		public enum CheckBoxFormFieldValue
		{
			True,
			False
		}
	}
}