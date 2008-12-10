using System;
using System.Web.UI;

namespace Zeus.ContentTypes
{
	internal class RootEditorContainer : EditorContainerAttribute
	{
		public RootEditorContainer()
			: base("Root", 0)
		{

		}

		public override Control AddTo(Control container)
		{
			return container;
		}
	}
}
