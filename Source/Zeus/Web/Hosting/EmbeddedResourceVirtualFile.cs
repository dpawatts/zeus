using System;
using System.Diagnostics;
using System.Reflection;

namespace Zeus.Web.Hosting
{
	[DebuggerDisplay("Assembly = {ContainingAssembly.FullName}, ResourcePath = {ResourcePath}, VirtualPath = {VirtualPath}")]
	public class EmbeddedResourceVirtualFile : System.Web.Hosting.VirtualFile
	{
		private readonly Assembly _containingAssembly;
		private readonly string _resourcePath;

		public Assembly ContainingAssembly
		{
			get { return _containingAssembly; }
		}

		public string ResourcePath
		{
			get { return _resourcePath; }
		}

		public EmbeddedResourceVirtualFile(string virtualPath, Assembly containingAssembly, string resourcePath)
			: base(virtualPath)
		{
			if (containingAssembly == null)
				throw new ArgumentNullException("containingAssembly");
			if (resourcePath == null)
				throw new ArgumentNullException("resourcePath");
			if (resourcePath.Length == 0)
				throw new ArgumentOutOfRangeException("resourcePath");
			_containingAssembly = containingAssembly;
			_resourcePath = resourcePath;
		}

		public override System.IO.Stream Open()
		{
			return _containingAssembly.GetManifestResourceStream(_resourcePath);
		}
	}
}