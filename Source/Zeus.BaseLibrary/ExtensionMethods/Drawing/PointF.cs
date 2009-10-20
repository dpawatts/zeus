using System.Drawing;

namespace Isis.ExtensionMethods.Drawing
{
	public static class PointFExtensionMethods
	{
		public static void Offset(this PointF point, float dx, float dy)
		{
			point.X += dx;
			point.Y += dy;
		}
	}
}
