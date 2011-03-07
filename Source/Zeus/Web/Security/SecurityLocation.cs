using System;
using System.Collections.Generic;

namespace Zeus.Web.Security
{
	public abstract class SecurityLocation
	{
		protected SecurityLocation()
		{
			ChildLocations = new List<SecurityLocation>();
		}

		public IList<SecurityLocation> ChildLocations { get; private set; }
		public string Path { get; set; }

		public SecurityLocation GetChild(string path)
		{
			if (string.IsNullOrEmpty(path))
				return this;

			int slashIndex = path.IndexOf('/');
			if (slashIndex == 0) // starts with slash
			{
				if (path.Length == 1)
					return this;
				return GetChild(path.Substring(1));
			}

			if (slashIndex > 0) // contains a slash further down
			{
				string nameSegment = path.Substring(0, slashIndex);
				foreach (SecurityLocation child in ChildLocations)
					if (LocationPathMatches(child.Path, path))
						return child.GetChild(path.Substring(slashIndex));
				return this;
			}

			// no slash, only a name
			foreach (SecurityLocation child in ChildLocations)
				if (LocationPathMatches(child.Path, path))
					return child;

			return this;
		}

		private static bool LocationPathMatches(string locationPath, string pathToCheck)
		{
			locationPath = locationPath.TrimStart('/');
			return pathToCheck.StartsWith(locationPath, StringComparison.OrdinalIgnoreCase);
		}
	}
}