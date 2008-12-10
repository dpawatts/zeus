using System;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace Zeus.ContentTypes.Properties
{
	/// <summary>
	/// Attribute used to mark properties as editable. This attribute is predefined to use 
	/// the <see cref="System.Web.UI.WebControls.TextBox"/> web control as editor.</summary>
	/// <example>
	/// [N2.Details.EditableTextBox("Published", 80)]
	/// public override DateTime Published
	/// {
	///     get { return base.Published; } 
	///     set { base.Published = value; }
	/// }
	/// </example>
	[AttributeUsage(AttributeTargets.Property)]
	public class TextBoxEditorAttribute : AbstractEditorAttribute
	{
		/// <summary>Initializes a new instance of the EditableTextBoxAttribute class.</summary>
		/// <param name="title">The label displayed to editors</param>
		/// <param name="sortOrder">The order of this editor</param>
		public TextBoxEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{
		}

		/// <summary>Initializes a new instance of the EditableTextBoxAttribute class.</summary>
		/// <param name="title">The label displayed to editors</param>
		/// <param name="sortOrder">The order of this editor</param>
		/// <param name="maxLength">The max length of the text box.</param>
		public TextBoxEditorAttribute(string title, int sortOrder, int maxLength)
			: this(title, sortOrder)
		{
			this.MaxLength = maxLength;
		}

		#region Properties

		/// <summary>Gets or sets columns on the text box.</summary>
		public int Columns
		{
			get;
			set;
		}

		/// <summary>Gets or sets rows on the text box.</summary>
		public int Rows
		{
			get;
			set;
		}

		/// <summary>Gets or sets the text box mode.</summary>
		public TextBoxMode TextMode
		{
			get;
			set;
		}

		/// <summary>Gets or sets the max length of the text box.</summary>
		public int MaxLength
		{
			get;
			set;
		}

		/// <summary>Gets or sets the default value. When the editor's value equals this value then null is saved instead.</summary>
		public string DefaultValue
		{
			get;
			set;
		}

		#endregion

		public override bool UpdateItem(ContentItem item, Control editor)
		{
			TextBox tb = editor as TextBox;
			string value = (tb.Text == DefaultValue) ? null : tb.Text;
			if (!AreEqual(value, item[this.Name]))
			{
				item[Name] = value;
				return true;
			}
			return false;
		}

		public override void UpdateEditor(ContentItem item, Control editor)
		{
			TextBox tb = editor as TextBox;
			tb.Text = Utility.Convert<string>(item[Name]) ?? DefaultValue;
		}

		/// <summary>Creates a text box editor.</summary>
		/// <param name="container">The container control the tetx box will be placed in.</param>
		/// <returns>A text box control.</returns>
		protected override Control AddEditor(Control container)
		{
			TextBox tb = CreateEditor();
			tb.ID = Name;
			tb.CssClass += " textEditor";
			ModifyEditor(tb);
			container.Controls.Add(tb);

			return tb;
		}

		/// <summary>Instantiates the text box control.</summary>
		/// <returns>A text box.</returns>
		protected virtual TextBox CreateEditor()
		{
			return new TextBox();
		}

		protected virtual void ModifyEditor(TextBox tb)
		{
			if (MaxLength > 0) tb.MaxLength = MaxLength;
			if (Columns > 0) tb.Columns = Columns;
			if (Rows > 0) tb.Rows = Rows;
			if (Columns > 0) tb.Rows = Rows;
			tb.TextMode = TextMode;
		}
	}
}
