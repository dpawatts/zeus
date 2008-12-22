using System;
using Zeus.Persistence;
using System.Diagnostics;
using Zeus.Integrity;
using Zeus.Web;
using System.Security.Permissions;
using Microsoft.Win32;
using System.IO;
using Zeus.FileSystem.Details;
using System.Web;

namespace Zeus.FileSystem
{
	[ContentType(Description = "A node that represents a file.")]
	[RestrictParents(typeof(Folder))]
	public class File : FileSystemNode
	{
		public override string IconUrl
		{
			get { return "~/Admin/Assets/Images/Icons/page_white.png"; }
		}

		public override string Url
		{
			get { return "~/File.axd?Path=" + HttpUtility.UrlEncode(base.Path); }
		}

		[UploadEditor(IsLocallyUnique = true)]
		public override string Name
		{
			get { return base.Name; }
			set { base.Name = value; }
		}

		public byte[] Data
		{
			get { return GetDetail<byte[]>("Data", null); }
			set { SetDetail<byte[]>("Data", value); }
		}

		public string ContentType
		{
			get { return GetDetail<string>("ContentType", null); }
			set { SetDetail<string>("ContentType", value); }
		}

		public long? Size
		{
			get { return GetDetail<long?>("Size", null); }
			set { SetDetail<long?>("Size", value); }
		}
	}
}
