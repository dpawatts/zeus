using System.Web.UI;
using Zeus.ContentTypes;
using Zeus.Web.UI.WebControls;

namespace Zeus.Web.UI
{
	public static class ControlExtensionMethods
	{
		public static ContentItem FindCurrentItem(this Control control)
		{
			IContentItemContainer container = FindParent<IContentItemContainer>(control.Parent);
			if (container != null)
				return container.CurrentItem;
			
			return null;
		}

		public static IEditableObject FindCurrentEditableObject(this Control control)
		{
			IEditableObjectEditor container = FindParent<IEditableObjectEditor>(control.Parent);
			if (container != null)
				return container.CurrentItem;

			return null;
		}

		public static T FindParent<T>(this Control control)
			where T : class
		{
			if (control == null || control is T)
				return control as T;
			else
				return FindParent<T>(control.Parent);
		}
	}
}
