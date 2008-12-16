using System;
using Zeus.Integrity;
using Zeus.Web;
using Zeus.ContentTypes.Properties;

namespace Zeus.FileSystem
{
	[ContentType("File Folder", "RootFolder", "A node that maps to files in the file system.", "", 600)]
	[RestrictParents(typeof(IFileSystemContainer))]
	public class RootFolder : BaseFolder
	{
		public RootFolder()
		{
			Visible = false;
			SortOrder = 10000;
		}

		private string physicalPath = null;
		public override string PhysicalPath
		{
			get
			{
				if (base.PhysicalPath == null)
				{
					Url u = Zeus.Web.Url.Parse("~/");
					base.PhysicalPath = GetWebContext().MapPath(u.AppendSegment(Name, string.Empty).ToString());
				}
				return base.PhysicalPath;
			}
			set { base.PhysicalPath = value; }
		}

		private IWebContext GetWebContext()
		{
			return Zeus.Context.Current.Resolve<IWebContext>();
		}
	}
}
