using System;
using Zeus.Persistence;
using Zeus.Web;

namespace Zeus.Admin
{
	public class Navigator
	{
		private readonly IPersister persister;
		private readonly Host host;

		public Navigator(IPersister persister, Host host)
		{
			this.persister = persister;
			this.host = host;
		}

		public ContentItem Navigate(ContentItem startingPoint, string path)
		{
			return startingPoint.GetChild(path);
		}

		public ContentItem Navigate(string path)
		{
			if (path == null) throw new ArgumentNullException("path");
			if (!path.StartsWith("/"))
			{
				if (path.StartsWith("~"))
					return Navigate(persister.Get(host.StartPageID), path.Substring(1));
				throw new ArgumentException("The path must start with a slash '/', was '" + path + "'", "path");
			}

			return Navigate(persister.Get(host.RootItemID), path);
		}
	}
}
