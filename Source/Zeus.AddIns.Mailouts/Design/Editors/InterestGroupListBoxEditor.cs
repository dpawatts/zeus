using System.Linq;
using System.Web.UI.WebControls;
using Zeus.AddIns.Mailouts.ContentTypes;
using Zeus.AddIns.Mailouts.Services;
using Zeus.ContentTypes;
using Zeus.Design.Editors;

namespace Zeus.AddIns.Mailouts.Design.Editors
{
	public class InterestGroupListBoxEditor : ListBoxEditorAttribute
	{
		public InterestGroupListBoxEditor(string title, int sortOrder)
			: base(title, sortOrder)
		{
			MultiSelect = true;
		}

		protected override ListItem[] GetListItems(IEditableObject item)
		{
			return ((Campaign) ((ContentItem) item).Parent).List.InterestGroups.Cast<string>()
				.Select(ig => new ListItem(ig)).ToArray();
		}
	}
}