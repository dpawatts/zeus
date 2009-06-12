using System.Web.UI;

namespace Zeus.Web.UI.WebControls
{
	public abstract class FileUploadImplementation
	{
		protected FileDataEditor OwnerControl { get; private set; }

		protected FileUploadImplementation(FileDataEditor ownerControl)
		{
			OwnerControl = ownerControl;
		}

		public abstract string StartUploadJavascriptFunction { get; }

		public virtual bool RendersSelf
		{
			get { return false; }
		}

		public abstract void AddChildControls();
		public abstract void Initialize();
	}
}