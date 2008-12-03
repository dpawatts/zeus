using System;
using System.Web.UI;
using Zeus.Definitions;

namespace Zeus.Edit
{
	public interface IEditable : IContainable
	{
		/// <summary>Gets or sets the label used for presentation.</summary>
		string Title { get; set; }

		/// <summary>Gets or sets the name of the prpoerty referenced by this attribute.</summary>
		string Name { get; set; }

		/// <summary>Updates the object with the values from the editor.</summary>
		/// <param name="item">The object to update.</param>
		/// <param name="editor">The editor contorl whose values to update the object with.</param>
		/// <returns>True if the item was changed (and needs to be saved).</returns>
		bool UpdateItem(ContentItem item, Control editor);

		/// <summary>Updates the editor with the values from the object.</summary>
		/// <param name="item">The object that contains values to assign to the editor.</param>
		/// <param name="editor">The editor to load with a value.</param>
		void UpdateEditor(ContentItem item, Control editor);
	}
}
