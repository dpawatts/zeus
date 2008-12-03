using System;

namespace Zeus.Definitions
{
	public interface IDefinitionRefiner
	{
		void Refine(ItemDefinition currentDefinition);
	}
}
