using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Isis.Web.UI
{
	[DebuggerDisplay("Assembly = {ContainingAssembly.GetName().Name}, ResourcePath = {ResourcePath}, VirtualPath = {VirtualPath}")]
	public class EmbeddedWebResourceFile
	{
		public EmbeddedWebResourceFile(string virtualPath, Assembly containingAssembly, string resourcePath, string contentType)
		{
			VirtualPath = virtualPath;
			ContainingAssembly = containingAssembly;
			ResourcePath = resourcePath;
			ContentType = contentType;
		}

		public Assembly ContainingAssembly { get; set; }
		public string ContentType { get; set; }
		public string ResourcePath { get; set; }
		public string VirtualPath { get; set; }

		public Stream Open()
		{
			return ContainingAssembly.GetManifestResourceStream(ResourcePath);
		}
	}
}