using System;
using System.Windows.Browser;

namespace Zeus.SilverlightUpload.Classes
{
	[ScriptableType]
	public class UploadEventArgs : EventArgs
	{
		public string FileName { get; set; }
		public string Identifier { get; set; }
	}
}