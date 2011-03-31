using System.Web.UI;
using System.Web.UI.WebControls;
using Zeus.ContentTypes;

namespace Zeus.Design.Editors
{
	public abstract class ListEditorAttribute : AbstractEditorAttribute
	{
		#region Constructors

		protected ListEditorAttribute()
		{
		}

		protected ListEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{
		}

		#endregion

		protected override void DisableEditor(Control editor)
		{
			((ListControl) editor).Enabled = false;
		}

		public override bool UpdateItem(IEditableObject item, Control editor)
		{
			ListControl ddl = (ListControl) editor;
			if (GetValue(ddl) != GetValue(item))
			{
				item[Name] = GetValue(ddl);
				return true;
			}
			return false;
		}

		/// <summary>Gets the object to store as content from the drop down list editor.</summary>
		/// <param name="ddl">The editor.</param>
		/// <returns>The value to store.</returns>
		protected virtual object GetValue(ListControl ddl)
		{
			return ddl.SelectedValue;
		}

		protected override void UpdateEditorInternal(IEditableObject item, Control editor)
		{
			ListControl ddl = (ListControl) editor;
			ddl.Items.AddRange(GetListItems(item));
			if (item[Name] != null)
				SetValue(ddl, GetValue(item));
		}

		protected virtual void SetValue(ListControl editor, object value)
		{
			editor.SelectedValue = value.ToString();
		}

		/// <summary>Gets a string value for the drop down list editor from the content item.</summary>
		/// <param name="item">The item containing the value.</param>
		/// <returns>A string to use as selected value.</returns>
		protected virtual object GetValue(IEditableObject item)
		{
			return item[Name] as string;
		}

		protected override Control AddEditor(Control container)
		{
			ListControl ddl = CreateEditor();
			container.Controls.Add(ddl);
			ddl.ID = Name;
			if (!Required)
				ddl.Items.Add(new ListItem());

			ModifyEditor(ddl);

			return ddl;
		}

		protected abstract ListControl CreateEditor();
		protected virtual void ModifyEditor(ListControl listControl) {}

		protected abstract ListItem[] GetListItems(IEditableObject item);
	}
}