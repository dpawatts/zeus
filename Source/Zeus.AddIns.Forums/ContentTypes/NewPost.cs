using System.Web.UI.WebControls;
using Isis.Web.UI;
using Zeus.Design.Editors;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;
using Zeus.Web;

namespace Zeus.AddIns.Forums.ContentTypes
{
	[ContentType("New / Edit Post Page")]
	[RestrictParents(typeof(MessageBoard))]
	[Template("~/UI/Views/Forums/NewPost.aspx")]
	public class NewPost : BasePage
	{
		public NewPost()
		{
			Visible = false;
		}

		public override string IconUrl
		{
			get { return WebResourceUtility.GetUrl(typeof(Forum), "Zeus.AddIns.Forums.Web.Resources.comment_edit.png"); }
		}
	}
}