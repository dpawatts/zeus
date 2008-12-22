using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Isis.Reflection;

namespace Zeus.Integrity
{
	public class IntegrityManager : IIntegrityManager
	{
		/// <summary>Find out if an item name is occupied.</summary>
		/// <param name="name">The name to check.</param>
		/// <param name="item">The item whose siblings (other items with the same parent) might be have a clashing name.</param>
		/// <returns>True if the name is unique.</returns>
		public bool IsLocallyUnique(string name, ContentItem item)
		{
			ContentItem parentItem = item.Parent;
			if (parentItem != null)
				foreach (ContentItem potentiallyClashingItem in parentItem.Children)
					if (potentiallyClashingItem.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase) && !potentiallyClashingItem.Equals(item))
						return false;
			return true;
		}

		/// <summary>Find out if an arbitrary property on an item is already used by an item in the same scope.</summary>
		/// <param name="propertyName">The property to check.</param>
		/// <param name="value">The property value to check.</param>
		/// <param name="item">The item whose siblings (other items with the same parent) might be have a clashing property value.</param>
		/// <returns>True if the property value is unique.</returns>
		public bool IsLocallyUnique(string propertyName, object value, ContentItem item)
		{
			ContentItem parentItem = item.Parent;
			if (parentItem != null)
				foreach (ContentItem potentiallyClashingItem in parentItem.Children)
					if (!potentiallyClashingItem.Equals(item))
					{
						if (potentiallyClashingItem[propertyName] is string)
						{
							string potentiallyClashingValue = (string) potentiallyClashingItem[propertyName];
							if (potentiallyClashingValue.Equals(value as string, StringComparison.InvariantCultureIgnoreCase))
								return false;
						}
						else if (potentiallyClashingItem[propertyName].Equals(value))
							return false;
					}
			return true;
		}
	}
}
