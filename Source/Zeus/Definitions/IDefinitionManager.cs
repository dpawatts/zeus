using System;
using System.Collections.Generic;

namespace Zeus.Definitions
{
	public interface IDefinitionManager
	{
		ItemDefinition this[Type type]
		{
			get;
		}

		ItemDefinition this[string discriminator]
		{
			get;
		}

		ContentItem CreateInstance(Type itemType, ContentItem parentItem);
		ICollection<ItemDefinition> GetDefinitions();
	}
}
