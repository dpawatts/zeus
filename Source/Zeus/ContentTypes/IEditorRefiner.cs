using System;

namespace Zeus.ContentTypes
{
	public interface IEditorRefiner
	{
		/// <summary>
		/// Allows the editor to update itself based on the type of property it will be editing
		/// </summary>
		/// <param name="propertyType"></param>
		void Refine(Type propertyType);
	}
}
