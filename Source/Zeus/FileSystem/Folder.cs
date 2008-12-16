using System;
using Zeus.Integrity;
using System.Diagnostics;
using Zeus.Web;
using System.IO;
using Zeus.Persistence;

namespace Zeus.FileSystem
{
	[RestrictParents(typeof(BaseFolder))]
	public class Folder : BaseFolder, ISelfPersister
	{
		public Folder()
		{

		}

		public Folder(BaseFolder parent, DirectoryInfo directory)
		{
			this.Name = directory.Name;
			this.Title = directory.Name;
			this.Updated = directory.LastWriteTime;
			this.Created = directory.CreationTime;
			this.PhysicalPath = directory.FullName;
			this.Parent = parent;
			((IUrlParserDependency) this).SetUrlParser(Zeus.Context.UrlParser);
		}

		public override void AddTo(ContentItem newParent)
		{
			if (newParent is BaseFolder)
			{
				BaseFolder dir = BaseFolder.EnsureFolder(newParent);

				string from = this.PhysicalPath;
				string to = System.IO.Path.Combine(dir.PhysicalPath, Name);
				if (System.IO.Directory.Exists(to))
					throw new NameOccupiedException(this, newParent);

				if (from != null)
					System.IO.Directory.Move(from, to);
				else
					System.IO.Directory.CreateDirectory(to);
				PhysicalPath = to;
				Parent = newParent;
			}
			else if (newParent != null)
			{
				new ZeusException(newParent + " is not a Folder. AddTo only works on folders.");
			}
		}

		#region ISelfPersister Members

		public void Save()
		{
			string expectedPath = System.IO.Path.Combine(Folder.PhysicalPath, Name);
			if (expectedPath != PhysicalPath)
			{
				try
				{
					if (PhysicalPath != null)
					{
						System.IO.Directory.Move(PhysicalPath, expectedPath);
					}
					else
					{
						System.IO.Directory.CreateDirectory(expectedPath);
					}
					PhysicalPath = expectedPath;
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
				System.IO.Directory.Delete(PhysicalPath);
			}
			catch (Exception ex)
			{
				Trace.TraceError(ex.ToString());
			}
		}

		public void MoveTo(ContentItem destination)
		{
			BaseFolder d = BaseFolder.EnsureFolder(destination);

			string from = PhysicalPath;
			string to = System.IO.Path.Combine(d.PhysicalPath, Name);
			if (System.IO.File.Exists(to))
				throw new NameOccupiedException(this, destination);

			try
			{
				System.IO.Directory.Move(from, to);
				PhysicalPath = to;
				Parent = destination;
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
				System.IO.Directory.CreateDirectory(to);
				Folder copy = (Folder) destination.GetChild(Name);
				foreach (Folder childDir in GetDirectories())
					childDir.CopyTo(copy);
				foreach (File f in GetFiles())
					f.CopyTo(copy);

				return copy;
			}
			catch (Exception ex)
			{
				Trace.TraceError(ex.ToString());
				return this;
			}
		}

		#endregion
	}
}
