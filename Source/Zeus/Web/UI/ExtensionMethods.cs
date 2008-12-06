using System;
using System.Web.UI;

namespace Zeus.Web.UI
{
	public static class ExtensionMethods
	{
		public static ContentItem FindCurrentItem(this Control control)
		{
			IContentItemContainer container = FindParentContainer(control.Parent);
			return (container != null) ? container.CurrentItem : null;
		}

		private static IContentItemContainer FindParentContainer(Control control)
		{
			if (control == null || control is IContentItemContainer)
				return control as IContentItemContainer;
			else
				return FindParentContainer(control.Parent);
		}
	}
}
