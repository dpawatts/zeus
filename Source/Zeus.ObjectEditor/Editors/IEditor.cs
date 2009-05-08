using System;
using System.Web.UI;

namespace Zeus.ObjectEditor.Editors
{
	public interface IEditor : IContainable, IComparable<IEditor>
	{
		Type PropertyType { get; set; }

		bool Shared { get; set; }

		/// <summary>Gets or sets the label used for presentation.</summary>
		string Title { get; set; }

		/// <summary>Updates the object with the values from the editor.</summary>
		/// <param name="item">The object to update.</param>
		/// <param name="editor">The editor contorl whose values to update the object with.</param>
		/// <returns>True if the item was changed (and needs to be saved).</returns>
		bool UpdateObject(IEditableObject item, Control editor);

		/// <summary>Updates the editor with the values from the object.</summary>
		/// <param name="item">The object that contains values to assign to the editor.</param>
		/// <param name="editor">The editor to load with a value.</param>
		void UpdateEditor(IEditableObject item, Control editor);
	}
}