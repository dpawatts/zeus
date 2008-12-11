using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeus.Web.UI.WebControls;

[assembly: WebResource("NameEditorAttribute.js", "text/javascript")]
namespace Zeus.ContentTypes.Properties
{
	public class NameEditorAttribute : AbstractEditorAttribute
	{
		public NameEditorAttribute(string title, int sortOrder)
			: base(title, "Name", sortOrder)
		{
			this.Required = true;
		}

		protected override Control AddEditor(Control container)
		{
			NameEditor nameEditor = new NameEditor { ID = "txtNameEditor" };
			if (this.Required)
				nameEditor.CssClass += " required";
			container.Controls.Add(nameEditor);
			return nameEditor;
		}

		public override bool UpdateItem(ContentItem item, Control editor)
		{
			item.Name = ((NameEditor) editor).Text;
			return true;
		}

		public override void UpdateEditor(ContentItem item, Control editor)
		{
			((NameEditor) editor).Text = item.Name;
		}
	}
}