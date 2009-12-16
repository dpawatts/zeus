using System.Web.UI.WebControls;
using Zeus.Web.Security;

namespace Zeus.Design.Editors
{
	public class PasswordEditorAttribute : TextBoxEditorAttribute
	{
		protected override void UpdateEditorInternal(Zeus.ContentTypes.IEditableObject item, System.Web.UI.Control editor)
		{
			
		}

		public override bool UpdateItem(ContentTypes.IEditableObject item, System.Web.UI.Control editor)
		{
			TextBox tb = editor as TextBox;
			string value = (tb.Text == DefaultValue) ? null : tb.Text;
			value = Context.Current.Resolve<ICredentialService>().EncryptPassword(value);
			if (!AreEqual(value, item[Name]))
			{
				item[Name] = value;
				return true;
			}
			return false;
		}
	}
}