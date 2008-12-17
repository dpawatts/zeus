using System;
using System.Web.UI;
using Zeus.Web.UI.WebControls;

namespace Zeus.ContentTypes.Properties
{
	public class ComboBoxEditorAttribute : AbstractEditorAttribute
	{
		#region Constructors

		public ComboBoxEditorAttribute()
		{
		}

		public ComboBoxEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{
		}

		#endregion

		protected override Control AddEditor(Control container)
		{
			ComboBox comboBox = new ComboBox();
			container.Controls.Add(comboBox);
			return comboBox;
		}

		public override bool UpdateItem(ContentItem item, Control editor)
		{
			return false;
		}

		public override void UpdateEditor(ContentItem item, Control editor)
		{
			
		}
	}
}
