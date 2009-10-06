using System.Reflection;

namespace Zeus.Web.Hosting
{
	public interface IEmbeddedResourceManager
	{
		EmbeddedResourceVirtualFile GetFile(string path);
		bool FileExists(string path);

		string GetClientResourceUrl(Assembly resourceAssembly, string relativePath);
		string GetServerResourceUrl(Assembly resourceAssembly, string relativePath);
	}
}