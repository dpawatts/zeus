using System;
using System.Web.UI;

namespace Zeus.Web.UI
{
	public static class ControlExtensionMethods
	{
		public static ContentItem FindCurrentItem(this Control control)
		{
			IContentItemContainer container = FindParent<IContentItemContainer>(control.Parent);
			return (container != null) ? container.CurrentItem : null;
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
