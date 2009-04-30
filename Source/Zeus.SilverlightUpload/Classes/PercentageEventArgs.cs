using System;
using System.Windows.Browser;

namespace Zeus.SilverlightUpload.Classes
{
	[ScriptableType]
	public class PercentageEventArgs : EventArgs
	{
		public int Percentage { get; set; }
	}
}