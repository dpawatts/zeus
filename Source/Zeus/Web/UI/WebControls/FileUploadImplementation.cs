using System.Web.UI;

namespace Zeus.Web.UI.WebControls
{
	public abstract class FileUploadImplementation
	{
		protected Control OwnerControl { get; private set; }

		protected FileUploadImplementation(Control ownerControl)
		{
			OwnerControl = ownerControl;
		}

		public abstract string StartUploadJavascriptFunction { get; }

		public abstract void AddChildControls();
		public abstract void Initialize();
	}
}