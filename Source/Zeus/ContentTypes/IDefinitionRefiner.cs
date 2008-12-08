using System;

namespace Zeus.ContentTypes
{
	public interface IDefinitionRefiner
	{
		void Refine(ContentType currentDefinition);
	}
}
