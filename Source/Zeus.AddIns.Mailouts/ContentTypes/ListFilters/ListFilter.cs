using Zeus.AddIns.Mailouts.Services;
using Zeus.Integrity;

namespace Zeus.AddIns.Mailouts.ContentTypes.ListFilters
{
	[RestrictParents(typeof(Campaign))]
	public abstract class ListFilter : ContentItem
	{
		public abstract bool Matches(IMailoutRecipient recipient);
	}
}