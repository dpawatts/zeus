using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeus.Design.Editors;
using System.Web.UI.WebControls;
using Zeus.Linq;
using System.Web.UI;
using Zeus.ContentTypes;

namespace Zeus.Examples.MinimalMvcExample.Design.Editors
{
    public class PageListBoxEditorAttribute : ListBoxEditorAttribute
    {
        #region Constructors

        public PageListBoxEditorAttribute()
		{
            MultiSelect = true;
		}

        public PageListBoxEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{
            MultiSelect = true;
		}

		#endregion

        protected override ListControl CreateEditor()
        {
            return new ListBox() { Rows = 10 };
        }

        protected override Control AddEditor(Control container)
        {
            ListControl ddl = CreateEditor();
            container.Controls.Add(ddl);
            ddl.ID = Name;
                        
            ModifyEditor(ddl);

            return ddl;
        }

        protected override object GetValue(ListControl ddl)
        {
           return ddl
                .Items
                .Cast<ListItem>()
                .Where(li => li.Selected)
                .Select(li => int.Parse(li.Value))
                .ToList();
        }

        protected override object GetValue(IEditableObject item)
        {
            string [] result = new string [0];

            var linkedItems = item[Name] as IList<int>;
            if (linkedItems != null)
                result = linkedItems.Select(p => p.ToString()).ToArray();

            return result;
        }

        protected override ListItem[] GetListItems(IEditableObject item)
        {
            return Zeus.Find.EnumerateAccessibleChildren(Zeus.Find.StartPage).NavigablePages()
                .ToList()
                .OrderBy(i => i.HierarchicalTitle)
                .Select(i => new ListItem { Value = i.ID.ToString(), Text = i.Parent.Title + " - " + i.Title })
                .ToArray();
        }
    }
}
