using System;
using System.Web.UI.WebControls;

namespace Zeus.ContentTypes.Properties
{
	public class LiteralDisplayerAttribute : DisplayerAttribute
	{
		public LiteralDisplayerAttribute()
			: base(typeof(Literal), "Text")
		{

		}
	}
}
