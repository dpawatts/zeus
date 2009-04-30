using System.Collections.Generic;

namespace Zeus.ContentTypes
{
	/// <summary>
	/// Disables a definition removing it from lists when choosing new items. 
	/// Existing items will not be affaceted.
	/// </summary>
	public class DisableAttribute : AbstractContentTypeRefiner, IDefinitionRefiner
	{
		public override void Refine(ContentType currentDefinition, IList<ContentType> allDefinitions)
		{
			currentDefinition.Enabled = false;
		}
	}
}