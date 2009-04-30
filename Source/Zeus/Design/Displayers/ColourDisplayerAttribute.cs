using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zeus.Design.Displayers
{
	public class ColourDisplayerAttribute : DisplayerAttribute
	{
		private Panel _panel;

		public override void InstantiateIn(Control container)
		{
			_panel = new Panel();
			_panel.Width = Unit.Pixel(50);
			_panel.Height = Unit.Pixel(17);
			container.Controls.Add(_panel);
		}

		public override void SetValue(Control container, ContentItem item, string propertyName)
		{
			string hexRef = item[propertyName] as string;
			if (hexRef != null && hexRef.Length == 6)
			{
				_panel.BackColor = ColorTranslator.FromHtml("#" + hexRef);
				_panel.ToolTip = hexRef;
			}
		}
	}
}