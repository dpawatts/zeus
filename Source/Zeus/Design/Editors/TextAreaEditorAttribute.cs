using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using Zeus.BaseLibrary.ExtensionMethods;
using Zeus.ContentTypes;

namespace Zeus.Design.Editors
{
	/// <summary>
	/// Attribute used to mark properties as editable. This attribute is predefined to use 
	/// the <see cref="System.Web.UI.WebControls.TextBox"/> web control as editor.</summary>
	/// <example>
	/// [Zeus.Details.EditableTextBox("Published", 80)]
	/// public override DateTime Published
	/// {
	///     get { return base.Published; } 
	///     set { base.Published = value; }
	/// }
	/// </example>
	[AttributeUsage(AttributeTargets.Property)]
	public class TextAreaEditorAttribute : TextEditorAttributeBase
	{
		private string _dataTypeText, _dataTypeErrorMessage;

		public TextAreaEditorAttribute()
		{
		}

		/// <summary>Initializes a new instance of the EditableTextBoxAttribute class.</summary>
		/// <param name="title">The label displayed to editors</param>
		/// <param name="sortOrder">The order of this editor</param>
		public TextAreaEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{
		}

		/// <summary>Initializes a new instance of the EditableTextBoxAttribute class.</summary>
		/// <param name="title">The label displayed to editors</param>
		/// <param name="sortOrder">The order of this editor</param>
		/// <param name="maxLength">The max length of the text box.</param>
		public TextAreaEditorAttribute(string title, int sortOrder, int maxLength)
			: base(title, sortOrder, maxLength)
		{

		}

		#region Properties

		/// <summary>Gets or sets height on the text box.</summary>
		public int Height { get; set; }

		/// <summary>Gets or sets width on the text box.</summary>
		public int Width { get; set; }

		public string TextBoxCssClass
		{
			get;
			set;
		}

		#endregion

		protected override void DisableEditor(Control editor)
		{
			((TextArea) editor).Enabled = false;
			((TextArea) editor).ReadOnly = true;
		}

		/// <summary>Creates a text box editor.</summary>
		/// <param name="container">The container control the tetx box will be placed in.</param>
		/// <returns>A text box control.</returns>
		protected override Control AddEditor(Control container)
		{
			TextArea tb = CreateEditor();
			tb.ID = Name;
			if (ReadOnly)
				tb.ReadOnly = true;
			ModifyEditor(tb);
			container.Controls.Add(tb);

			return tb;
		}

		/// <summary>Instantiates the text box control.</summary>
		/// <returns>A text box.</returns>
		protected virtual TextArea CreateEditor()
		{
			return new TextArea();
		}

		protected virtual void ModifyEditor(TextFieldBase tb)
		{
			if (MaxLength > 0) tb.MaxLength = MaxLength;
			if (Width > 0) tb.Width = Width;
			if (Height > 0) tb.Height = Height;
		}
	}
}