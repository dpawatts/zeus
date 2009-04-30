using System.Collections.Generic;

namespace Zeus.ContentTypes
{
	public interface IDefinitionRefiner
	{
		void Refine(ContentType currentContentType, IList<ContentType> allContentTypes);
	}
}