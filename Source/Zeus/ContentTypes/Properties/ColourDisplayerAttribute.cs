using System;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Drawing;

namespace Zeus.ContentTypes.Properties
{
	public class ColourDisplayerAttribute : DisplayerAttribute
	{
		public override Control AddTo(Control container, ContentItem item, string propertyName)
		{
			string hexRef = item[propertyName] as string;
			Control result = null;
			if (hexRef != null)
			{
				Panel panel = new Panel();
				panel.BackColor = ColorTranslator.FromHtml("#" + hexRef);
				panel.Width = Unit.Pixel(50);
				panel.Height = Unit.Pixel(17);
				panel.ToolTip = hexRef;
				container.Controls.Add(panel);
				result = panel;
			}
			return result;
		}
	}
}
