using System;

namespace Zeus.Integrity
{
	public interface IIntegrityManager
	{
		/// <summary>Checks wether an item's name is locally unique, i.e. no other sibling has the same name.</summary>
		/// <param name="name">The name we're proposing for the item.</param>
		/// <param name="item">The item whose siblings to check.</param>
		/// <returns>True if the item would get a unique name.</returns>
		bool IsLocallyUnique(string name, ContentItem item);

		/// <summary>Find out if an arbitrary property on an item is already used by an item in the same scope.</summary>
		/// <param name="propertyName">The property to check.</param>
		/// <param name="value">The property value to check.</param>
		/// <param name="item">The item whose siblings (other items with the same parent) might be have a clashing property value.</param>
		/// <returns>True if the property value is unique.</returns>
		bool IsLocallyUnique(string propertyName, object value, ContentItem item);
	}
}
