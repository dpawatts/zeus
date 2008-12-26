using System;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Zeus.Web.UI.WebControls
{
	public class Span : HtmlGenericControl
	{
		public Span()
			: base("span")
		{

		}

		public string CssClass
		{
			get { return this.Attributes["class"]; }
			set { this.Attributes["class"] = value; }
		}
	}
}
