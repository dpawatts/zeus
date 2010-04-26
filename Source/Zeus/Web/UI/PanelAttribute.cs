using System.Web.UI;
using Ext.Net;
using Zeus.ContentTypes;

namespace Zeus.Web.UI
{
	/// <summary>Defines a fieldset that can contain editors when editing an item.</summary>
	public class PanelAttribute : EditorContainerAttribute
	{
		/// <summary>Gets or sets the fieldset legend (text/title).</summary>
		public string Title { get; set; }

		public bool Collapsible { get; set; }
		public bool Collapsed { get; set; }

		public PanelAttribute(string name, string title, int sortOrder)
			: base(name, sortOrder)
		{
			Title = title;
		}

		/// <summary>Adds the fieldset to a parent container and returns it.</summary>
		/// <param name="container">The parent container onto which to add the container defined by this interface.</param>
		/// <returns>The newly added fieldset.</returns>
		public override Control AddTo(Control container)
		{
			Panel fieldSet = new Panel
												{
													ID = "Panel" + Name,
													Title = Title,
													//Collapsible = Collapsible,
													//Collapsed = Collapsed,
													BodyStyle = "padding:5px",
													StyleSpec = "margin-bottom:10px;",
													AutoHeight = false,
													AutoScroll = true,
													MaxHeight = 300,
													Height = 300
												};
			container.Controls.Add(fieldSet);
			return fieldSet;
		}
	}
}