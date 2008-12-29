using System;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Drawing;

namespace Zeus.ContentTypes.Properties
{
	public class DateDisplayerAttribute : DisplayerAttribute
	{
		private Literal _literal;

		public override void InstantiateIn(Control container)
		{
			_literal = new Literal();
			container.Controls.Add(_literal);
		}

		public override void SetValue(Control container, ContentItem item, string propertyName)
		{
			DateTime date = (DateTime) item[propertyName];
			_literal.Text = date.ToShortDateString();
		}
	}
}
