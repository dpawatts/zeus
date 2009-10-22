using System;
using System.Web.UI.WebControls;
using System.Web.UI;
using Zeus.BaseLibrary.Web.UI.HtmlControls;

namespace Zeus.Web.UI.WebControls
{
	public class ExtendedListView : System.Web.UI.WebControls.ListView
	{
		private LinkButton _sortButton;

		/// <summary>
		/// Captures a Delete command from a command button within the ListView. This code
		/// assumes that each row contains a checkbox with an ID set to 'chkDelete'.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="e"></param>
		/// <returns></returns>
		protected override bool OnBubbleEvent(object source, EventArgs e)
		{
			if (e is CommandEventArgs)
			{
				switch (((CommandEventArgs) e).CommandName)
				{
					case "Delete":
						bool result = false;
						// loop through rows and delete item if checkbox is checked
						foreach (ListViewDataItem listViewDataItem in Items)
						{
							CheckBox deleteCheckBox = listViewDataItem.FindControl("chkDelete") as CheckBox;
							if (deleteCheckBox != null)
							{
								result = true;
								if (deleteCheckBox.Checked)
									DeleteItem(listViewDataItem.DisplayIndex);
							}
						}
						if (result)
							return true;
						break;
					case "Sort":
						_sortButton = source as LinkButton;
						break;
				}
			}

			return base.OnBubbleEvent(source, e);
		}

		protected override void OnSorted(EventArgs e)
		{
			if (_sortButton != null)
			{
				SortableColumnHeader sortableColumnHeader = _sortButton.Parent as SortableColumnHeader;
				if (sortableColumnHeader != null)
				{
					sortableColumnHeader.IncludedInSort = true;
					sortableColumnHeader.SortDirection = this.SortDirection;

					// reset other column headers
					foreach (Control control in sortableColumnHeader.Parent.Controls)
						if (control is SortableColumnHeader && control != sortableColumnHeader)
							((SortableColumnHeader) control).IncludedInSort = false;
				}
			}
		}
	}
}