using System;
using System.Collections;
using System.Web.Caching;
using System.Web.Hosting;

namespace Zeus.Web.Hosting
{
	public class EmbeddedResourcePathProvider : VirtualPathProvider
	{
		private readonly IEmbeddedResourceManager _manager;

		public EmbeddedResourcePathProvider(IEmbeddedResourceManager manager)
		{
			_manager = manager;
		}

		public override bool FileExists(string virtualPath)
		{
			if (virtualPath == null)
				throw new ArgumentNullException("virtualPath");
			if (virtualPath.Length == 0)
				throw new ArgumentOutOfRangeException("virtualPath");
			string absolutePath = System.Web.VirtualPathUtility.ToAbsolute(virtualPath);
			if (_manager.FileExists(absolutePath))
				return true;
			return base.FileExists(absolutePath);
		}

		public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
		{
			if (virtualPath == null)
				throw new ArgumentNullException("virtualPath");
			if (virtualPath.Length == 0)
				throw new ArgumentOutOfRangeException("virtualPath");

			string absolutePath = System.Web.VirtualPathUtility.ToAbsolute(virtualPath);

			// Lazy initialize the return value so we can return null if needed
			AggregateCacheDependency retVal = null;

			// Handle chained dependencies
			if (virtualPathDependencies != null)
				foreach (string virtualPathDependency in virtualPathDependencies)
				{
					CacheDependency dependencyToAdd = GetCacheDependency(virtualPathDependency, null, utcStart);
					if (dependencyToAdd == null) // Ignore items that have no dependency
						continue;

					if (retVal == null)
						retVal = new AggregateCacheDependency();
					retVal.Add(dependencyToAdd);
				}

			// Handle the primary file
			CacheDependency primaryDependency = null;
			primaryDependency = FileHandledByBaseProvider(absolutePath)
				? base.GetCacheDependency(absolutePath, null, utcStart)
				: new CacheDependency((_manager.GetFile(absolutePath)).ContainingAssembly.Location, utcStart);

			if (primaryDependency != null)
			{
				if (retVal == null)
					retVal = new AggregateCacheDependency();
				retVal.Add(primaryDependency);
			}

			return retVal;
		}

		public override VirtualFile GetFile(string virtualPath)
		{
			// virtualPath comes in absolute form: /MyApplication/Subfolder/OtherFolder/Control.ascx
			// * ToAppRelative: ~/Subfolder/OtherFolder/Control.ascx
			// * ToAbsolute: /MyApplication/Subfolder/OtherFolder/Control.ascx
			if (virtualPath == null)
				throw new ArgumentNullException("virtualPath");
			if (virtualPath.Length == 0)
				throw new ArgumentOutOfRangeException("virtualPath");

			string absolutePath = System.Web.VirtualPathUtility.ToAbsolute(virtualPath);
			if (FileHandledByBaseProvider(absolutePath))
				return base.GetFile(absolutePath);
			return _manager.GetFile(absolutePath);
		}

		/// <summary>
		/// Determines if a file should be handled by the base provider or if
		/// it should be handled by this provider.
		/// </summary>
		/// <param name="absolutePath">The absolute path to the file to check.</param>
		/// <returns>
		/// <see langword="true" /> if processing of the file at
		/// <paramref name="absolutePath" /> should be done by the base provider;
		/// <see langword="false" /> if this provider should handle it.
		/// </returns>
		/// <seealso cref="EmbeddedResourcePathProvider" />
		private bool FileHandledByBaseProvider(string absolutePath)
		{
			return !_manager.FileExists(absolutePath);
		}
	}
}