using System;
using Zeus.ContentTypes;

namespace Zeus.Web.UI.WebControls
{
	/// <summary>
	/// Event argument containing item data.
	/// </summary>
	public class ItemViewTypeDefinitionEventArgs : EventArgs
	{
		public ItemViewTypeDefinitionEventArgs(ITypeDefinition typeDefinition)
		{
			TypeDefinition = typeDefinition;
		}

		/// <summary>Gets or sets the item associated with these arguments.</summary>
		public ITypeDefinition TypeDefinition { get; set; }
	}
}