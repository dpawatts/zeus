using System.Web;
using Zeus.FileSystem.Details;
using Zeus.Integrity;

namespace Zeus.FileSystem
{
	[ContentType(Description = "A node that represents a file.")]
	[RestrictParents(typeof(Folder))]
	public class File : FileSystemNode
	{
		public override string IconUrl
		{
			get
			{
				switch (FileExtension)
				{
					case ".pdf":
						return "~/Admin/Assets/Images/Icons/page_white_acrobat.png";
					case ".doc":
					case ".docx":
						return "~/Admin/Assets/Images/Icons/page_white_word.png";
					default:
						return "~/Admin/Assets/Images/Icons/page_white.png";
				}
			}
		}

		public override string Url
		{
			get { return "~/File.axd?Path=" + HttpUtility.UrlEncode(Path); }
		}

		[UploadEditor(IsLocallyUnique = true)]
		public override string Name
		{
			get { return base.Name; }
			set { base.Name = value; }
		}

		public string FileExtension
		{
			get { return System.IO.Path.GetExtension(Name); }
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
