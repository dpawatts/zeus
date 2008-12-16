using System;
using System.Collections.Generic;
using Zeus.Linq.Filters;
using System.Linq;
using System.IO;
using System.Diagnostics;
using Zeus.ContentTypes.Properties;
using Zeus.Integrity;

namespace Zeus.FileSystem
{
	[ContentType("File Folder", "Folder", "A node that stores files and other folders.", "", 600)]
	[RestrictParents(typeof(IFileSystemContainer), typeof(Folder))]
	public class Folder : FileSystemNode
	{
		public override string IconUrl
		{
			get { return "~/Admin/Assets/Images/Icons/folder.png"; }
		}

		[TextBoxEditor("Name", 10)]
		public override string Name
		{
			get { return base.Name; }
			set { base.Name = value; }
		}
	}
}
