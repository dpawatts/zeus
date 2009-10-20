using System;
using System.Web.UI.WebControls;

namespace Isis.Web.UI.WebControls
{
	public class BorderlessImageButton : ImageButton
	{
		public override Unit BorderWidth
		{
			get
			{
				if (base.BorderWidth.IsEmpty)
					return Unit.Pixel(0);
				else
					return base.BorderWidth;
			}
			set
			{
				base.BorderWidth = value;
			}
		}
	}
}
