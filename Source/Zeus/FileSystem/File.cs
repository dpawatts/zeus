using System;
using Zeus.Persistence;
using System.Diagnostics;
using Zeus.Integrity;
using Zeus.Web;
using System.Security.Permissions;
using Microsoft.Win32;
using System.IO;
using Zeus.FileSystem.Details;

namespace Zeus.FileSystem
{
	[RestrictParents(typeof(BaseFolder))]
	public class File : FileSystemNode, ISelfPersister
	{
		public File()
		{

		}

		public File(BaseFolder parent, FileInfo file)
		{
			this.Name = file.Name;
			this.Title = file.Name;
			this.Size = file.Length;
			this.Updated = file.LastWriteTime;
			this.Created = file.CreationTime;
			this.PhysicalPath = file.FullName;
			this.Parent = parent;
			((IUrlParserDependency) this).SetUrlParser(Zeus.Context.UrlParser);
		}

		[UploadEditor]
		public override string Name
		{
			get { return base.Name; }
			set { base.Name = value; }
		}

		public long Size
		{
			get;
			set;
		}

		public override string IconUrl
		{
			get { return "~/Admin/Assets/Images/Icons/page_white.png"; }
		}

		public string GetMimeType()
		{
			string fileExtension = this.Extension.ToLower();

			RegistryPermission registryPermission = new RegistryPermission(RegistryPermissionAccess.Read, @"\\HKEY_CLASSES_ROOT");
			RegistryKey classesRoot = Registry.ClassesRoot;
			RegistryKey typeKey = classesRoot.OpenSubKey(@"MIME\Database\Content Type");
			foreach (string keyName in typeKey.GetSubKeyNames())
			{
				RegistryKey currentKey = typeKey.OpenSubKey(keyName);
				if (((string) currentKey.GetValue("Extension")).ToLower() == fileExtension)
					return keyName;
			}

			return string.Empty;
		}

		public override void AddTo(ContentItem newParent)
		{
			if (newParent != null)
				MoveTo(newParent);
		}

		public void Save()
		{
			string expectedPath = System.IO.Path.Combine(this.Folder.PhysicalPath, this.Name);
			if (expectedPath != this.PhysicalPath)
			{
				try
				{
					if (this.PhysicalPath != null)
						System.IO.Directory.Move(this.PhysicalPath, expectedPath);
					else
						System.IO.Directory.CreateDirectory(expectedPath);
					this.PhysicalPath = expectedPath;
				}
				catch (Exception ex)
				{
					Trace.TraceError(ex.ToString());
				}
			}
		}

		public void Delete()
		{
			try
			{
				System.IO.File.Delete(this.PhysicalPath);
			}
			catch (Exception ex)
			{
				Trace.TraceError(ex.ToString());
			}
		}

		public void MoveTo(ContentItem destination)
		{
			BaseFolder d = BaseFolder.EnsureFolder(destination);

			string from = this.PhysicalPath;
			string to = System.IO.Path.Combine(d.PhysicalPath, Name);
			if (System.IO.File.Exists(to))
				throw new NameOccupiedException(this, destination);

			try
			{
				System.IO.File.Move(from, to);
				this.PhysicalPath = to;
				this.Parent = destination;
			}
			catch (Exception ex)
			{
				Trace.TraceError(ex.ToString());
			}
		}

		public ContentItem CopyTo(ContentItem destination)
		{
			BaseFolder d = BaseFolder.EnsureFolder(destination);

			string from = PhysicalPath;
			string to = System.IO.Path.Combine(d.PhysicalPath, Name);
			if (System.IO.File.Exists(to))
				throw new NameOccupiedException(this, destination);


			try
			{
				System.IO.File.Copy(from, to);
				return (File) destination.GetChild(Name);
			}
			catch (Exception ex)
			{
				Trace.TraceError(ex.ToString());
				return this;
			}
		}
	}
}
