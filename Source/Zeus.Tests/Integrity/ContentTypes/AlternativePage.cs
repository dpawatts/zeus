using Zeus.Integrity;

namespace Zeus.Tests.Integrity.ContentTypes
{
	[RestrictParents(typeof(AlternativeStartPage))] // SubClassOfRoot as parent allowed
	public class AlternativePage : ContentItem
	{

	}
}