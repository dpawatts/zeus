using System.Web.UI.WebControls;

namespace Zeus.Design.Displayers
{
	public class LiteralDisplayerAttribute : DisplayerAttribute
	{
		public LiteralDisplayerAttribute()
			: base(typeof (Literal), "Text")
		{
		}
	}
}