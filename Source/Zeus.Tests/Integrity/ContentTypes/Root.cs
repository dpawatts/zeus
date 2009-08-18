using Zeus.Integrity;

namespace Zeus.Tests.Integrity.ContentTypes
{
	[ContentType]
	[AllowedChildren(typeof(StartPage))]
	public class Root : ContentItem
	{

	}
}