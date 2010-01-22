using System.Web.UI;
using Ext.Net;
using Zeus.ContentTypes;

namespace Zeus.Web.UI
{
	/// <summary>Defines a fieldset that can contain editors when editing an item.</summary>
	public class FieldSetAttribute : EditorContainerAttribute
	{
		/// <summary>Gets or sets the fieldset legend (text/title).</summary>
		public string Legend { get; set; }

		public bool Collapsible { get; set; }
		public bool Collapsed { get; set; }

		public FieldSetAttribute(string name, string legend, int sortOrder)
			: base(name, sortOrder)
		{
			Legend = legend;
		}

		/// <summary>Adds the fieldset to a parent container and returns it.</summary>
		/// <param name="container">The parent container onto which to add the container defined by this interface.</param>
		/// <returns>The newly added fieldset.</returns>
		public override Control AddTo(Control container)
		{
			FieldSet fieldSet = new FieldSet
			{
				ID = "FieldSet" + Name,
				Title = Legend,
				Collapsible = Collapsible,
				Collapsed = Collapsed
			};
			container.Controls.Add(fieldSet);
			return fieldSet;
		}
	}
}