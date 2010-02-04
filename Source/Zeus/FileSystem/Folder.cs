using Ext.Net;
using Zeus.BaseLibrary.Web.UI;
using Zeus.Design.Editors;
using Zeus.Integrity;

namespace Zeus.FileSystem
{
	[ContentType("File Folder", "Folder", "A node that stores files and other folders.", "", 600)]
	[RestrictParents(typeof(IFileSystemContainer), typeof(Folder))]
	public class Folder : FileSystemNode
	{
		public Folder()
		{
			SortOrder = int.MaxValue;
			Visible = false;
		}

		protected override Icon Icon
		{
			get { return Icon.Folder; }
		}

		[TextBoxEditor("Name", 10)]
		public override string Name
		{
			get { return base.Name; }
			set { base.Name = value; }
		}
	}
}
