using System.Web.UI;
using Zeus.ContentTypes;
using Zeus.Web.UI.HtmlControls;

namespace Zeus.Web.UI
{
	/// <summary>Defines a fieldset that can contain editors when editing an item.</summary>
	public class FieldSetAttribute : EditorContainerAttribute
	{
		public FieldSetAttribute(string name, string legend, int sortOrder)
			: base(name, sortOrder)
		{
			Legend = legend;
		}

		/// <summary>Gets or sets the fieldset legend (text/title).</summary>
		public string Legend { get; set; }

		/// <summary>Adds the fieldset to a parent container and returns it.</summary>
		/// <param name="container">The parent container onto which to add the container defined by this interface.</param>
		/// <returns>The newly added fieldset.</returns>
		public override Control AddTo(Control container)
		{
			HtmlFieldSet fieldSet = new HtmlFieldSet { ID = Name, Legend = Legend };
			container.Controls.Add(fieldSet);
			return fieldSet;
		}
	}
}