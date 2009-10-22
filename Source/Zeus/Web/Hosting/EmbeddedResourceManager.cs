using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Routing;
using Zeus.BaseLibrary.Web;

namespace Zeus.Web.Hosting
{
	/// <summary>
	/// Manages embedded *.aspx and *.ascx files, such as those used
	/// in the admin site.
	/// </summary>
	public class EmbeddedResourceManager : IEmbeddedResourceManager
	{
		private readonly Dictionary<string, Assembly> _assemblyPathPrefixes;
		private readonly Dictionary<Assembly, string> _reverseAssemblyPathPrefixes;
		private readonly Dictionary<Assembly, string> _clientResourcePrefixes;
		private readonly Dictionary<string, EmbeddedResourceVirtualFile> _files;
		private readonly IWebContext _webContext;

		public EmbeddedResourceManager(IEmbeddedResourceBuilder builder, IWebContext webContext)
		{
			_assemblyPathPrefixes = builder.ResourceSettings.AssemblyPathPrefixes;
			_reverseAssemblyPathPrefixes = _assemblyPathPrefixes.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
			_clientResourcePrefixes = builder.ResourceSettings.ClientResourcePrefixes;
			_files = new Dictionary<string, EmbeddedResourceVirtualFile>();
			_webContext = webContext;
		}

		public EmbeddedResourceVirtualFile GetFile(string path)
		{
			return GetOrCreateVirtualFile(path, true);
		}

		public bool FileExists(string path)
		{
			return GetOrCreateVirtualFile(path, false) != null;
		}

		public string GetClientResourceUrl(Assembly resourceAssembly, string relativePath)
		{
			return "/content/" + _reverseAssemblyPathPrefixes[resourceAssembly]
				+ "/" + relativePath;
		}

		public string GetServerResourceUrl(Assembly resourceAssembly, string resourcePath)
		{
			// Resource path is in the form Zeus.Admin.Children.aspx.
			// First, take assembly name off the front.
			string assemblyName = resourceAssembly.GetName().Name;
			if (!resourcePath.StartsWith(assemblyName))
				throw new ArgumentException(string.Format("Resource path '{0}' must start with assembly name '{1}'.", resourcePath, assemblyName), "resourcePath");

			string relativePath = resourcePath.Substring(assemblyName.Length + 1);
			if (relativePath.EndsWith(".aspx"))
			{
				relativePath = Regex.Replace(relativePath, "^([A-Z])", m => m.Groups[1].Value.ToLower());
				relativePath = Regex.Replace(relativePath, "([a-z])([A-Z])", m => m.Groups[1].Value + "-" + m.Groups[2].Value.ToLower());
				relativePath = Regex.Replace(relativePath, "([A-Z])", m => m.Groups[1].Value.ToLower());
			}

			return "/" + _reverseAssemblyPathPrefixes[resourceAssembly] + "/" + relativePath;
		}

		private EmbeddedResourceVirtualFile GetOrCreateVirtualFile(Url url, bool throwOnError)
		{
			// Always deal with lower-case URLs for aspx pages.
			if (url.Extension == ".aspx")
				url = url.ToString().ToLower();

			// First check if we already have a virtual file cached.
			if (_files.ContainsKey(url))
				return _files[url];

			// Grab the first segment of the path. This will be the assembly prefix.
			string assemblyPathPrefix = url.SegmentAtIndex(0);
			if (string.IsNullOrEmpty(assemblyPathPrefix))
				if (throwOnError)
					throw new ArgumentException("URL does not contain an assembly path prefix", "url");
				else
					return null;
			if (!_assemblyPathPrefixes.ContainsKey(assemblyPathPrefix))
				if (throwOnError)
					throw new ArgumentException("URL does not contain a valid assembly path prefix", "url");
				else
					return null;

			Assembly assembly = _assemblyPathPrefixes[assemblyPathPrefix];

			// Now get the rest of the path. This, combined with the assembly prefix, will be the resource path.
			Url remainingUrl = url.RemoveSegment(0);
			string resourcePath = remainingUrl.PathWithoutExtension.Replace('/', '.');
			if (remainingUrl.Extension == ".aspx")
			{
				resourcePath = Regex.Replace(resourcePath, "[^a-zA-Z]([a-z])|^([a-z])", m => m.Captures[0].Value.ToUpper());
				resourcePath = Regex.Replace(resourcePath, "-", string.Empty);
			}

			// Create a new virtual file.
			EmbeddedResourceVirtualFile virtualFile = new EmbeddedResourceVirtualFile(url, assembly, assembly.GetName().Name + resourcePath + remainingUrl.Extension);
			_files.Add(url, virtualFile);

			// Check that resource actually exists.
			if (assembly.GetManifestResourceStream(virtualFile.ResourcePath) == null)
				throw new ArgumentException(string.Format("Cannot find resource in assembly '{0}' matching resource path '{1}'.", assembly, virtualFile.ResourcePath));

			return virtualFile;
		}
	}
}