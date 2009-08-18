using Zeus.ContentProperties;
using Zeus.Design.Editors;
using Zeus.Integrity;

namespace Zeus.Tests.Integrity.ContentTypes
{
	[ContentType("Page", "ContentTypesPage")]
	[RestrictParents(typeof(StartPage))]
	public class Page : ContentItem
	{
		[TextBoxEditor("My Property", 100)]
		[PropertyAuthorizedRoles("ACertainGroup")]
		public virtual string MyProperty
		{
			get { return (string)(GetDetail("MyProperty") ?? ""); }
			set { SetDetail("MyProperty", value); }
		}
	}
}