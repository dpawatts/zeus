using System;
using System.Linq;
using System.Web.UI.WebControls;
using Zeus.ContentTypes;
using System.Web.UI;

namespace Zeus.Design.Editors
{
	public abstract class ListBoxEditorAttribute : ListEditorAttribute
	{
		#region Constructors

		protected ListBoxEditorAttribute()
		{
		}

		protected ListBoxEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{
		}

		#endregion

		public bool MultiSelect { get; set; }

		protected override ListControl CreateEditor()
		{
			return new ListBox();
		}

		protected override void ModifyEditor(ListControl listControl)
		{
			((ListBox) listControl).SelectionMode = (MultiSelect) ? ListSelectionMode.Multiple : ListSelectionMode.Single;
		}

		protected override object GetValue(ListControl ddl)
		{
			return ddl.Items.Cast<ListItem>().Where(li => li.Selected).Select(li => li.Value).ToArray();
		}

		protected override object GetValue(IEditableObject item)
		{
			return item[Name] as string[];
		}

		protected override void SetValue(ListControl listEditor, object value)
		{
			string[] values = (string[]) value;
			foreach (string forEachValue in values)
			{
				ListItem listItem = listEditor.Items.FindByValue(forEachValue);
				if (listItem != null)
					listItem.Selected = true;
			}
		}

        public override bool UpdateItem(IEditableObject item, Control editor)
        {
            //check if there's a value yet...
            if (item[Name] == null)
                return true;
            else
            {
                ListControl ddl = (ListControl)editor;

                string selectedInEditor = "";
                foreach (ListItem listItem in ddl.Items)
                {
                    if (listItem.Selected)
                        selectedInEditor += "_" + listItem.Value;
                }

                System.Collections.Generic.List<int> orig_ddl = (System.Collections.Generic.List<int>)item[Name];
                string selectedInObject = "";
                foreach (int val in orig_ddl)
                {
                    selectedInObject += "_" + val;
                }

                if (selectedInEditor != selectedInObject)
                {
                    item[Name] = GetValue(ddl);
                    return true;
                }
            }
            return false;
        }
	}
}