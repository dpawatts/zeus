using System;

namespace Zeus.SilverlightUpload.Classes
{
	public class StateEventArgs : EventArgs
	{
		public Constants.FileStates State { get; set; }
	}
}