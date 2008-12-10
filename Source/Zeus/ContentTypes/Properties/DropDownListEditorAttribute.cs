using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zeus.ContentTypes.Properties
{
	public abstract class DropDownListEditorAttribute : AbstractEditorAttribute
	{
		#region Constructors

		public DropDownListEditorAttribute()
		{
		}

		public DropDownListEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{
		}

		#endregion

		public override bool UpdateItem(ContentItem item, Control editor)
		{
			DropDownList ddl = (DropDownList) editor;
			if (ddl.SelectedValue != GetValue(item))
			{
				item[Name] = GetValue(ddl);
				return true;
			}
			return false;
		}

		/// <summary>Gets the object to store as content from the drop down list editor.</summary>
		/// <param name="ddl">The editor.</param>
		/// <returns>The value to store.</returns>
		protected virtual object GetValue(DropDownList ddl)
		{
			return ddl.SelectedValue;
		}

		public override void UpdateEditor(ContentItem item, Control editor)
		{
			DropDownList ddl = (DropDownList) editor;
			ddl.Items.AddRange(GetListItems(item));
			if (item[Name] != null)
				ddl.SelectedValue = GetValue(item);
		}

		/// <summary>Gets a string value from the drop down list editor from the content item.</summary>
		/// <param name="item">The item containing the value.</param>
		/// <returns>A string to use as selected value.</returns>
		protected virtual string GetValue(ContentItem item)
		{
			return item[Name] as string;
		}

		protected override Control AddEditor(Control container)
		{
			DropDownList ddl = new DropDownList();
			ddl.ID = Name;
			if (!this.Required)
				ddl.Items.Add(new ListItem());

			
			container.Controls.Add(ddl);

			return ddl;
		}

		protected abstract ListItem[] GetListItems(ContentItem item);
	}
}
