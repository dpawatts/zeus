using System.Web.UI.WebControls;
using Zeus;
using Zeus.Web;
using Zeus.Integrity;
using Zeus.Design.Editors;
using Zeus.ContentProperties;
using Zeus.Templates.ContentTypes;
using Zeus.Web.UI;

namespace Zeus.Examples.MinimalMvcExample.ContentTypes
{
	[ContentType("My Little Type")]
	public class MyLittleType : BaseContentItem
	{
		[ContentProperty("Test String", 10, Shared = false)]
		public virtual string TestString
		{
			get { return GetDetail("TestString", string.Empty); }
			set { SetDetail("TestString", value); }
		}

        /*
		[XhtmlStringContentProperty("Test Rich String", 35)]
		public virtual string TestRichString
		{
			get { return GetDetail("TestRichString", string.Empty); }
			set { SetDetail("TestRichString", value); }
		}
         */

		[ContentProperty("Multi Line Textbox", 35)]
		[TextAreaEditor(Height = 200, Width = 500)]
		public virtual string MultiTextBox
		{
			get { return GetDetail("MultiTextBox", string.Empty); }
			set { SetDetail("MultiTextBox", value); }
		}
        
	}
}