using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Isis;
using Isis.ExtensionMethods;

namespace Zeus.ObjectEditor.ValueEditors
{
	public class TextBoxEditor : BaseValueEditor
	{
		#region Fields

		private readonly string _name, _title;
		private string _dataTypeText, _dataTypeErrorMessage;
		private readonly Type _propertyType;

		#endregion

		#region Constructor

		public TextBoxEditor(string name, string title, Type propertyType)
		{
			_name = name;
			_title = title;
			_propertyType = propertyType;
		}

		#endregion

		#region Properties

		/// <summary>Gets or sets columns on the text box.</summary>
		public int Columns { get; set; }

		/// <summary>Gets or sets rows on the text box.</summary>
		public int Rows { get; set; }

		/// <summary>Gets or sets the text box mode.</summary>
		public TextBoxMode TextMode { get; set; }

		/// <summary>Gets or sets the max length of the text box.</summary>
		public int MaxLength { get; set; }

		public string TextBoxCssClass { get; set; }

		public string DataTypeText
		{
			get { return _dataTypeText ?? "&nbsp;*"; }
			set { _dataTypeText = value; }
		}

		public string DataTypeErrorMessage
		{
			get { return _dataTypeErrorMessage ?? string.Format("{0} must be a valid {1}", _title, GetDataTypeName()); }
			set { _dataTypeErrorMessage = value; }
		}

		/// <summary>Gets or sets the default value. When the editor's value equals this value then null is saved instead.</summary>
		public string DefaultValue { get; set; }

		#endregion

		public override Control CreateEditor()
		{
			TextBox textBox = CreateEditorInternal();
			textBox.ID = _name;
			textBox.CssClass += " textEditor " + TextBoxCssClass;
			if (Required)
				textBox.CssClass += " required";
			ModifyEditor(textBox);

			return textBox;
		}

		/// <summary>Instantiates the text box control.</summary>
		/// <returns>A text box.</returns>
		protected virtual TextBox CreateEditorInternal()
		{
			return new TextBox();
		}

		private string GetDataTypeName()
		{
			Type propertyType = _propertyType.GetTypeOrUnderlyingType();
			if (propertyType == typeof(int))
				return "integer";
			if (propertyType == typeof(decimal) || propertyType == typeof(double) || propertyType == typeof(float))
				return "number";
			if (propertyType == typeof(DateTime))
				return "date";
			throw new NotSupportedException();
		}

		public override void AddValidators(Control panel, Control editor)
		{
			// If data type is not string, we need to add a validator for data type
			Type realType = _propertyType.GetTypeOrUnderlyingType();
			if (realType == typeof(int) || realType == typeof(decimal) || realType == typeof(double) || realType == typeof(float) || realType == typeof(DateTime))
				AddCompareValidator(panel, editor);
		}

		protected virtual IValidator AddCompareValidator(Control container, Control editor)
		{
			CompareValidator cmv = new CompareValidator
			{
				ID = "cmv" + _name,
				ControlToValidate = editor.ID,
				Display = ValidatorDisplay.Dynamic,
				Text = DataTypeText,
				ErrorMessage = DataTypeErrorMessage,
				Type = GetValidationDataType(),
				Operator = ValidationCompareOperator.DataTypeCheck
			};
			container.Controls.Add(cmv);

			return cmv;
		}

		private ValidationDataType GetValidationDataType()
		{
			Type propertyType = _propertyType.GetTypeOrUnderlyingType();
			if (propertyType == typeof(int))
				return ValidationDataType.Integer;
			if (propertyType == typeof(decimal) || propertyType == typeof(double) || propertyType == typeof(float))
				return ValidationDataType.Double;
			if (propertyType == typeof(DateTime))
				return ValidationDataType.Date;
			throw new NotSupportedException();
		}

		public override void DisableEditor(Control editor)
		{
			TextBox tb = (TextBox) editor;
			tb.Enabled = false;
			tb.ReadOnly = true;
		}

		protected virtual void ModifyEditor(TextBox tb)
		{
			if (MaxLength > 0) tb.MaxLength = MaxLength;
			if (Columns > 0) tb.Columns = Columns;
			if (Rows > 0) tb.Rows = Rows;
			if (Columns > 0) tb.Rows = Rows;
			tb.TextMode = TextMode;
		}

		public override object GetEditorValue(Control editor)
		{
			TextBox tb = (TextBox) editor;
			return (tb.Text == DefaultValue) ? null : tb.Text;
		}

		public override void SetEditorValue(Control editor, object value)
		{
			TextBox tb = (TextBox) editor;
			tb.Text = ConvertUtility.Convert<string>(value) ?? DefaultValue;
		}
	}
}