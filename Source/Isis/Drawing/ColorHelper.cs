using System;
using System.Drawing;
using System.Globalization;

namespace Isis.Drawing
{
	/// <summary>
	/// Summary description for Colour.
	/// </summary>
	public sealed class ColorHelper
	{
		public static bool Compare(Color tColour1, Color tColour2)
		{
			// check if both are totally transparent
			if (tColour1.A == 0 && tColour2.A == 0)
			{
				return true;
			}
			else
			{
				// otherwise, check individual components
				return (tColour1.A == tColour2.A &&
					tColour1.R == tColour2.R &&
					tColour1.G == tColour2.G &&
					tColour1.B == tColour2.B);
			}
		}

		public static Color Add(Color tColour1, Color tColour2)
		{
			if (tColour1.A == 0)
			{
				return tColour2;
			}
			else if (tColour2.A == 0)
			{
				return tColour1;
			}
			else
			{
				float fA1 = tColour1.A / 255.0f;
				float fR1 = tColour1.R / 255.0f;
				float fG1 = tColour1.G / 255.0f;
				float fB1 = tColour1.B / 255.0f;
				float fA2 = tColour2.A / 255.0f;
				float fR2 = tColour2.R / 255.0f;
				float fG2 = tColour2.G / 255.0f;
				float fB2 = tColour2.B / 255.0f;
				float fAlpha = fA1 * fA2;

				return Color.FromArgb(
					(int) (((fR1 * fAlpha) + (fR2 * (1.0f - fAlpha))) * 255.0f),
					(int) (((fG1 * fAlpha) + (fG2 * (1.0f - fAlpha))) * 255.0f),
					(int) (((fB1 * fAlpha) + (fB2 * (1.0f - fAlpha))) * 255.0f));
			}
		}

		public static Color FromHexRef(string sHexRef)
		{
			// get rid of # at start
			if (sHexRef.StartsWith("#")) sHexRef = sHexRef.Remove(0, 1);

			// check for valid length
			if (sHexRef.Length != 6)
			{
				throw new InvalidOperationException("Invalid hexref");
			}

			// get individual colour components
			string sR = sHexRef.Substring(0, 2);
			string sG = sHexRef.Substring(2, 2);
			string sB = sHexRef.Substring(4, 2);

			return Color.FromArgb(
				int.Parse(sR, NumberStyles.AllowHexSpecifier),
				int.Parse(sG, NumberStyles.AllowHexSpecifier),
				int.Parse(sB, NumberStyles.AllowHexSpecifier));
		}

		public static string ToHexRef(Color tColour)
		{
			return tColour.R.ToString("X2")
				+ tColour.G.ToString("X2")
				+ tColour.B.ToString("X2");
		}
	}
}
