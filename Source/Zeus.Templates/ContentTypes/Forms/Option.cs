using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.Templates.Mvc.ContentTypes.Forms
{
	[ContentType]
	[RestrictParents(typeof(OptionSelectQuestion))]
	public class Option : BaseWidget
	{

	}
}